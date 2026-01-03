namespace Moodle.Application.DTOs.Statistics
{
    public class StatisticsDTO
    {
        public int TotalStudents { get; set; }
        public int TotalProfessors { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalCourses { get; set; }

        public List<TopCourseDTO> TopCourses { get; set; } = new();

        public List<TopUserDTO> TopMessageSenders { get; set; } = new();
    }

    public class TopCourseDTO
    {
        public string CourseName { get; set; } = null!;
        public int StudentCount { get; set; }
    }

    public class TopUserDTO
    {
        public string UserName { get; set; } = null!;
        public int MessageCount { get; set; }
    }
}