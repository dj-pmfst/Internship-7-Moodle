namespace Moodle.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Textt { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public DateTime RecievedAt { get; set; }

        public User Sender {  get; set; }
        public User Receiver { get; set; }
    }
}
