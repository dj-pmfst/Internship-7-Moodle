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
        public bool IsAdmin() => Role == Roles.admin;
        public bool IsStudent() => Role == Roles.student;
        public bool CanTeachCourse() => Role == Roles.profesor || Role == Roles.admin;

        public ICollection<CourseEnrollment> Courses { get; set; }
        public ICollection<Course> TaughtCourses { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> RecievedMessages { get; set; }
        public ICollection<Announcement> Announcements { get; set; }

        public void Deactivate()
        {
            IsActive = false;
        }
        public void Activate()
        {
            IsActive = true;
        }
        public void PromoteToProfessor()
        {
            if (Role == Roles.student)
                Role = Roles.profesor;
        }
        public void DemoteToStudent()
        {
            if (Role == Roles.profesor)
                Role = Roles.student;
        }
    }
}
