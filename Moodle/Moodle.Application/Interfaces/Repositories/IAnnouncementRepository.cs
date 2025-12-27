using Moodle.Domain.Entities;

namespace Moodle.Application.Interfaces.Repositories
{
    public interface IAnnouncementRepository
    {
        Task<Announcement?> GetByIdAsync(int id);
        Task<IEnumerable<Announcement>> GetByCourseAsync(int courseId); 
        Task<IEnumerable<Announcement>> GetByProfessorAsync(int professorId);
        Task AddAsync(Announcement announcement);
        void Update(Announcement announcement);
        void Delete(Announcement announcement);
    }
}
