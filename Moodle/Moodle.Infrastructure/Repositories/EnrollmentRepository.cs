using Microsoft.EntityFrameworkCore;
using Moodle.Application.Interfaces.Repositories;
using Moodle.Domain.Entities;
using Moodle.Infrastructure.Persistence;
namespace Moodle.Infrastructure.Repositories
{
    public class EnrollmentRepository : Repository<CourseEnrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(MoodleDbContext context) : base(context) 
        {
        }

        public async Task<CourseEnrollment?> GetEnrollmentAsync(int studentId, int courseId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ce => ce.StudentId == studentId 
                    && ce.CourseId == courseId);
        }

        public async Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByStudentAsync(int studentId)
        {
            return await _dbSet
                .Where(ce => ce.StudentId == studentId)
                .Include(ce => ce.Course)
                .ToListAsync();
        }
        public async Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseAsync(int courseId)
        {
            return await _dbSet
                .Where(ce => ce.CourseId == courseId)
                .Include(ce => ce.Student)
                .ToListAsync();
        }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return await _dbSet
                .AnyAsync(ce => ce.StudentId == studentId 
                    && ce.CourseId == courseId);
        }

        public async Task<int> GetEnrollmentCountAsync(int courseId)
        {
            return await _dbSet
                .CountAsync(ce => ce.CourseId == courseId);
        }

    }
}
