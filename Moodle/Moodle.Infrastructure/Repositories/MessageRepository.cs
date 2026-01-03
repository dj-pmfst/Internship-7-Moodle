using Microsoft.EntityFrameworkCore;
using Moodle.Application.DTOs.Statistics;
using Moodle.Application.Interfaces.Repositories;
using Moodle.Domain.Entities;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(MoodleDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(int userId1, int userId2)
        {
            return await _dbSet
                .Where(m =>
                    (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                    (m.SenderId == userId2 && m.ReceiverId == userId1))
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(int userId1, int userId2)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetConversationPartnersAsync(int userId)
        {
            var sentToUsers = await _dbSet
                .Where(m => m.SenderId == userId && m.ReceiverId != null)
                .Select(m => m.Receiver)
                .Distinct()
                .ToListAsync();

            var receivedFromUsers = await _dbSet
                .Where(m => m.ReceiverId == userId && m.SenderId != null)
                .Select(m => m.Sender)
                .Distinct()
                .ToListAsync();

            return sentToUsers
                .Union(receivedFromUsers)
                .Where(u => u != null)
                .Distinct()
                .ToList();
        }

        public async Task<IEnumerable<User>> GetUsersWithoutConversationAsync(int userId)
        {
            var conversationPartners = await GetConversationPartnersAsync(userId);
            var conversationPartnerIds = conversationPartners.Select(u => u.Id).ToHashSet();

            return await _context.Users
                .Where(u => u.Id != userId
                    && !conversationPartnerIds.Contains(u.Id)
                    && !u.IsDeleted) 
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<int> GetMessageCountByUserAsync(int userId)
        {
            return await _dbSet
                .CountAsync(m => m.SenderId == userId);
        }
        public async Task<List<TopUserDTO>> GetTopMessageSendersAsync(int topN, DateTime? fromDate)
        {
            var query = _context.Messages.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(m => m.SentAt >= fromDate.Value);

            return await query
                .GroupBy(m => m.Sender.Name)
                .Select(g => new TopUserDTO
                {
                    UserName = g.Key,
                    MessageCount = g.Count()
                })
                .OrderByDescending(x => x.MessageCount)
                .Take(topN)
                .ToListAsync();
        }
    }
}