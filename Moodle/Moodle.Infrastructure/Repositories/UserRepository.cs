using Microsoft.EntityFrameworkCore;
using Moodle.Application.Interfaces.Repositories;
using Moodle.Domain.Entities;
using Moodle.Domain.Enums;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MoodleDbContext context) : base(context)
        {
        }
        public async Task<User?> GetByNameAsync (string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Name == name);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(Roles role)
        {
            return await _dbSet
                .Where(u => u.Role == role)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllStudentsAsync()
        {
            return await GetByRoleAsync(Roles.student);
        }

        public async Task<IEnumerable<User>> GetAllProfessorsAsync()
        {
            return await GetByRoleAsync(Roles.profesor);
        }
    }
}
