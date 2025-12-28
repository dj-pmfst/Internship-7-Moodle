using Microsoft.EntityFrameworkCore;
using Moodle.Application.Interfaces.Repositories;
using Moodle.Domain.Entities;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories
{
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(MoodleDbContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<Announcement>> GetByCourseAsync(int courseId)
        {
            return await _dbSet
                .Where(a => a.CourseId == courseId)
                .Include(a => a.Professor)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        public async Task<IEnumerable<Announcement>> GetByProfessorAsync(int professorId)
        {
            return await _dbSet
                .Where(a => a.ProfessorId == professorId)
                .Include(a => a.Course)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
