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
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public bool IsAdmin() => Role == Roles.admin;
        public bool IsStudent() => Role == Roles.student;
        public bool CanTeachCourse() => Role == Roles.profesor || Role == Roles.admin;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CourseEnrollment> Enrollments { get; set; }
        public ICollection<Course> TaughtCourses { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        public ICollection<Announcement> Announcements { get; set; }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
        public void PromoteToProfessor()
        {
            if (Role == Roles.student)
                Role = Roles.profesor;
            UpdatedAt = DateTime.UtcNow;
        }
        public void DemoteToStudent()
        {
            if (Role == Roles.profesor)
                Role = Roles.student;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
