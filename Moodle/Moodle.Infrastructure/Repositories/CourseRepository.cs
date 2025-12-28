using Microsoft.EntityFrameworkCore;
using Moodle.Application.Interfaces.Repositories;
using Moodle.Domain.Entities;
using Moodle.Infrastructure.Persistence;

namespace Moodle.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(MoodleDbContext context) : base(context) 
        { 
        }

        public async Task<Course?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<Course>> GetCoursesByProfessorAsync(int professorId)
        {
            return await _dbSet
                .Where(c => c.ProfessorId == professorId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<IEnumerable<Course>> GetCoursesByStudentAsync(int studentId)
        {
            return await _dbSet
                .Where(c => c.Enrollments.Any(e => e.StudentId == studentId))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetCourseStudentsAsync(int courseId)
        {
            return await _context.Enrollments
                .Where(ce => ce.CourseId == courseId)
                .Include(ce => ce.Student)
                .Select(ce => ce.Student)
                .OrderBy(s => s.Name)  
                .ToListAsync();
        }

        public async Task<bool> IsStudentEnrolledAsync(int courseId, int studentId)
        {
            return await _context.Enrollments
                .AnyAsync(ce => ce.CourseId == courseId && ce.StudentId == studentId);
        }
    }
}
