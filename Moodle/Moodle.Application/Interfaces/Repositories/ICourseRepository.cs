using Moodle.Domain.Entities;

namespace Moodle.Application.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(int id);
        Task<Course?> GetByNameAsync(string name);
        Task<IEnumerable<Course>> GetCoursesByProfessorAsync(int professorId); 
        Task<IEnumerable<Course>> GetCoursesByStudentAsync(int studentId);
        Task<IEnumerable<User>> GetCourseStudentsAsync(int courseId);
        Task<bool> IsStudentEnrolledAsync(int courseId, int studentId);
        Task<IEnumerable<Course>> GetAllWithProfessorAsync();
        Task<IEnumerable<Course>> GetAllAsync();

        Task AddAsync(Course course);
        void Update(Course course);
        void Delete(Course course);
    }
}
