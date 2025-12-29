namespace Moodle.Application.DTOs.Material
{
    public class AddMaterialRequest
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
