using Moodle.Domain.Entities;

namespace Moodle.Application.Interfaces.Repositories
{
    public interface IMaterialRepository
    {
        Task<Material?> GetByIdAsync(int id);
        Task<IEnumerable<Material>> GetByCourseAsync(int courseId); 
        Task AddAsync(Material material);
        void Update(Material material);
        void Delete(Material material);
    }
}
