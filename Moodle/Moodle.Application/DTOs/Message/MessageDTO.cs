namespace Moodle.Application.DTOs.Message
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public DateTime SentAt { get; set; }
        public string FormattedDate => SentAt.ToString("dd.MM.yyyy HH:mm");
        public bool IsSentByCurrentUser { get; set; }
    }
}
