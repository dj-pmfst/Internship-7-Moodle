using Moodle.Application.DTOs.Announcement;
using Moodle.Application.DTOs.Course;
using Moodle.Application.DTOs.Material;
using Moodle.Application.DTOs.User;
using Moodle.Application.Interfaces;
using Moodle.Domain.Entities;
using Moodle.Moodle.Application.Common;

namespace Moodle.Application.Services
{
    public class CourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesByStudentAsync(int studentId)
        {
            var courses = await _unitOfWork.Courses.GetCoursesByStudentAsync(studentId);

            return courses.Select(c => MapToDto(c));
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesByProfessorAsync(int professorId)
        {
            var courses = await _unitOfWork.Courses.GetCoursesByProfessorAsync(professorId);

            return courses.Select(c => MapToDto(c));
        }

        public async Task<CourseDetailsDTO> GetCourseDetailsAsync(int courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);

            if (course == null)
            {
                return null;
            }

            var students = await _unitOfWork.Courses.GetCourseStudentsAsync(courseId);
            var announcements = await _unitOfWork.Announcements.GetByCourseAsync(courseId);
            var materials = await _unitOfWork.Materials.GetByCourseAsync(courseId);

            return new CourseDetailsDTO
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                ProfessorName = course.Professor?.Name ?? "Unknown",
                Students = students.Select(s => new UserDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Role = s.Role,
                    IsActive = s.IsActive
                }).ToList(),
                Announcements = announcements.Select(a => new AnnouncementDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    Text = a.Text,
                    Professor = a.Professor?.Name ?? "Unknown",
                    CreatedAt = a.CreatedAt
                }).ToList(),
                Materials = materials.Select(m => new MaterialDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Url = m.Url,
                    CreatedAt = m.CreatedAt
                }).ToList()
            };
        }

        public async Task<ServiceResult<bool>> EnrollStudentAsync(EnrollStudentRequest request)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return ServiceResult<bool>.Failure("Kolegij nije pronađen");
            }

            var student = await _unitOfWork.Users.GetByIdAsync(request.StudentId);
            if (student == null)
            {
                return ServiceResult<bool>.Failure("Student nije pronađen");
            }

            if (await _unitOfWork.Courses.IsStudentEnrolledAsync(request.CourseId, request.StudentId))
            {
                return ServiceResult<bool>.Failure("Student je već upisan u ovaj kolegij");
            }

            var enrollment = new CourseEnrollment
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId
            };

            await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        private CourseDTO MapToDto(Course course)
        {
            return new CourseDTO
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Professor = course.Professor?.Name ?? "Unknown"
            };
        }
    }
}