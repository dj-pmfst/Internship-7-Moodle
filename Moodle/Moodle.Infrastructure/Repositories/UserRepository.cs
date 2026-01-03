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

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(Roles role)
        {
            return await _dbSet
                .Where(u => u.Role == role && !u.IsDeleted)
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

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            if (excludeUserId.HasValue)
            {
                return await _dbSet
                    .Where(u => !u.IsDeleted)
                    .AnyAsync(u => u.Email == email && u.Id != excludeUserId.Value);
            }
            return await _dbSet
                .Where(u => !u.IsDeleted)
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdIncludingDeletedAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<int> GetUserCountByRoleAsync(Roles role, DateTime? fromDate = null)
        {
            var query = _dbSet.Where(u => u.Role == role && !u.IsDeleted);

            if (fromDate.HasValue)
            {
                query = query.Where(u => u.CreatedAt >= fromDate.Value);
            }

            return await query.CountAsync();
        }
    }
}