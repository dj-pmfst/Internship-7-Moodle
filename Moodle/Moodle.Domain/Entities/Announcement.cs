namespace Moodle.Domain.Entities
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int? ProfessorId { get; set; }
        public User? Professor { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
