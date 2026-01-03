using Moodle.Application.DTOs.Statistics;
using Moodle.Application.Interfaces;
using Moodle.Domain.Enums;

namespace Moodle.Application.Services
{
    public class StatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StatisticsDTO> GetStatisticsAsync(DateTime? fromDate = null)
        {
            var statistics = new StatisticsDTO
            {
                TotalStudents = await _unitOfWork.Users.GetUserCountByRoleAsync(Roles.student, fromDate),
                TotalProfessors = await _unitOfWork.Users.GetUserCountByRoleAsync(Roles.profesor, fromDate),
                TotalAdmins = await _unitOfWork.Users.GetUserCountByRoleAsync(Roles.admin, fromDate),

                TotalCourses = await _unitOfWork.Courses.GetCourseCountAsync(fromDate),
            };

            var topCourses = await _unitOfWork.Courses.GetTopCoursesByEnrollmentAsync(3);
            statistics.TopCourses = topCourses.Select(c => new TopCourseDTO
            {
                CourseName = c.CourseName,
                StudentCount = c.StudentCount
            }).ToList();

            var topSenders = await _unitOfWork.Messages.GetTopMessageSendersAsync(3, fromDate);
            statistics.TopMessageSenders = topSenders.Select(u => new TopUserDTO
            {
                UserName = u.UserName,
                MessageCount = u.MessageCount
            }).ToList();

            return statistics;
        }
    }
}