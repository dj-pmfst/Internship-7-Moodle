using Moodle.Application.DTOs.Announcement;
using Moodle.Application.DTOs.Material;
using Moodle.Application.DTOs.User;

namespace Moodle.Application.DTOs.Course
{
    public class CourseDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProfessorName { get; set; }
        public List<UserDTO> Students { get; set; } = new();
        public List<AnnouncementDTO> Announcements { get; set; } = new();
        public List<MaterialDTO> Materials { get; set; } = new();
    }
}
