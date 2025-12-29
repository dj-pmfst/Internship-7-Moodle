using Moodle.Domain.Enums;

namespace Moodle.Moodle.Application.DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Roles Role { get; set; }
        public bool IsActive { get; set; }
        public string RoleDisplay => Role.ToString();
    }
}
