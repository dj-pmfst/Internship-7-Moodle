namespace Moodle.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ProfessorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? Professor { get; set; }
        public ICollection<Material> Materials { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
        public ICollection<CourseEnrollment> Enrollments { get; set; }

    }
}
