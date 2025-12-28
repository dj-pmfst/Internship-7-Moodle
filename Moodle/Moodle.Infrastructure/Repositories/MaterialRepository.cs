using Microsoft.EntityFrameworkCore;
using Moodle.Application.Interfaces.Repositories;
using Moodle.Domain.Entities;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(MoodleDbContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<Material>> GetByCourseAsync(int courseId)
        {
            return await _dbSet
                .Where(m => m.CourseID == courseId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
