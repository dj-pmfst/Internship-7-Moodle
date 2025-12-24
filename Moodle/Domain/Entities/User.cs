using Moodle.Domain.Enums;

namespace Moodle.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public bool isActive { get; set; } = true;
        public bool IsAdmin() => Role == Roles.admin;
        public bool CanTeachCourse() => Role == Roles.profesor || Role == Roles.admin;
    }
}
