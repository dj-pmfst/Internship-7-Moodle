using Moodle.Domain.Enums;

namespace Moodle.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Roles Role { get; set; }
        public bool IsActive { get; set; }
    }
}
