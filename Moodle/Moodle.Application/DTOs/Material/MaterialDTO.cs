namespace Moodle.Application.DTOs.Material
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FormattedDate => CreatedAt.ToString("dd.MM.yyyy HH:mm");
    }
}
