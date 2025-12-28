using Microsoft.EntityFrameworkCore.Storage;
using Moodle.Application.Interfaces;
using Moodle.Application.Interfaces.Repositories;
using Moodle.Infrastructure.Repositories;

namespace Moodle.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly MoodleDbContext _context;
        private IDbContextTransaction? _transaction;

        public IUserRepository Users { get; }
        public ICourseRepository Courses { get; }
        public IMessageRepository Messages { get; }
        public IAnnouncementRepository Announcements { get; }  
        public IMaterialRepository Materials {  get; }
        public IEnrollmentRepository Enrollments { get; }

        public UnitOfWork(MoodleDbContext context) 
        {
            _context = context;
            Users = new UserRepository(_context);
            Messages = new MessageRepository(_context);
            Courses = new CourseRepository(_context);
            Announcements = new AnnouncementRepository(_context);
            Materials = new MaterialRepository(_context);
            Enrollments = new EnrollmentRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();   
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null; 
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();    
        }
    }
}
