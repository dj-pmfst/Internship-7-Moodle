namespace Moodle.Domain.Entities
{
    public class CourseEnrollment
    {
        public int StudentId { get; set; }
        public User Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
