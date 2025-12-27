using Moodle.Domain.Entities;

namespace Moodle.Application.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<CourseEnrollment?> GetEnrollmentAsync(int studentId, int courseId);
        Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByStudentAsync(int studentId);
        Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseAsync(int courseId);  
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);
        Task<int> GetEnrollmentCountAsync(int courseId);
        Task AddAsync(CourseEnrollment enrollment);
        void Update(CourseEnrollment enrollment);
        void Delete(CourseEnrollment enrollment);

    }
}
