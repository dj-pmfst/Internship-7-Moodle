using Moodle.Application.DTOs.Statistics;
using Moodle.Domain.Entities;

namespace Moodle.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(int id);
        Task<IEnumerable<Message>> GetConversationAsync(int userId1, int userId2);
        Task<IEnumerable<User>> GetConversationPartnersAsync(int userId);
        Task<IEnumerable<User>> GetUsersWithoutConversationAsync(int userId);
        Task<IEnumerable<Message>> GetConversationMessagesAsync(int userId1, int userId2);
        Task<int> GetMessageCountByUserAsync(int userId);
        Task AddAsync(Message message);
        void Update(Message message);
        void Delete(Message message);
        Task<List<TopUserDTO>> GetTopMessageSendersAsync(int topN, DateTime? fromDate);
    }
}
