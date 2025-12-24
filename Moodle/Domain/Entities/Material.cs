namespace Moodle.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
