namespace Moodle.Application.DTOs.Announcement
{
    public class CreateAnnouncementRequest
    {
        public int CourseId { get; set; }
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
