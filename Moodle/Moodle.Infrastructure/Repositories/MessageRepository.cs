using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<User>> GetConversationPartnersAsync(int userId)
        {
            var sentToUsers = await _dbSet
                .Where(m => m.SenderId == userId)
                .Select(m => m.Receiver)
                .Distinct()
                .ToListAsync();

            var receivedFromUsers = await _dbSet
                .Where(m => m.ReceiverId == userId)
                .Select(m => m.Sender)
                .Distinct()
                .ToListAsync();

            return sentToUsers
                .Union(receivedFromUsers)
                .OrderBy(u => u.Name)
                .ToList();
        }
        public async Task<IEnumerable<User>> GetUsersWithoutConversationAsync(int userId)
        {
            var conversationPartners = await GetConversationPartnersAsync(userId);
            var conversationPartnerIds = conversationPartners.Select(u => u.Id).ToHashSet();

            return await _context.Users
                .Where(u => u.Id != userId && !conversationPartnerIds.Contains(u.Id))
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<int> GetMessageCountByUserAsync(int userId)
        {
            return await _dbSet
                .CountAsync(m => m.SenderId == userId);
        }
    }
}
