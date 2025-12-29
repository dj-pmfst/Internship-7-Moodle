namespace Moodle.Moodle.Application.DTOs.Announcement
{
    public class AnnouncementDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Professor { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FormattedDate => CreatedAt.ToString("dd.MM.yyyy HH:mm");
    }
}
