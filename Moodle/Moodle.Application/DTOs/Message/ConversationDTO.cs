namespace Moodle.Application.DTOs.Message
{
    public class ConversationDTO
    {
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string LastMessageText { get; set; }
        public DateTime LastMessageTime { get; set; }
        public string FormattedDate => LastMessageTime.ToString("dd.MM.yyyy HH:mm");
    }
}
