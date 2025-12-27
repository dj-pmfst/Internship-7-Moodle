using Moodle.Application.Interfaces.Repositories;

namespace Moodle.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; }
        IMaterialRepository Materials { get; }
        IAnnouncementRepository Announcements { get; }
        IMessageRepository Messages { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
