using Moodle.Domain.Entities;
using Moodle.Domain.Enums;

namespace Moodle.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByNameAsync(string name);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(Roles role);
        Task<IEnumerable<User>> GetAllStudentsAsync();
        Task<IEnumerable<User>> GetAllProfessorsAsync();
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    }
}
