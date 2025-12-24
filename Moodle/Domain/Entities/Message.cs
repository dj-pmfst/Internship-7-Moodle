namespace Moodle.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Textt { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime RecievedAt { get; set; }
    }
}
