namespace Moodle.Application.DTOs.User
{
    public class UpdateUserEmailRequest
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; }   
    }
}
