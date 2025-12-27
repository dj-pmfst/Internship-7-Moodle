namespace Moodle.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int CourseID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Course> Course { get; set; }
    }
}
