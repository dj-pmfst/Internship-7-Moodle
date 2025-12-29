using Moodle.Application.DTOs.Announcement;
using Moodle.Application.Interfaces;
using Moodle.Domain.Entities;
using Moodle.Moodle.Application.Common;
using ValidationResult = Moodle.Domain.Common.Validations.ValidationResult;

namespace Moodle.Application.Services
{
    public class AnnouncementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnnouncementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<AnnouncementDTO>> CreateAnnouncementAsync(
            CreateAnnouncementRequest request)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                validationResult.AddError("TitleRequired", "Title is required");
            }

            if (string.IsNullOrWhiteSpace(request.Text))
            {
                validationResult.AddError("TextRequired", "Text is required");
            }

            if (!validationResult.IsValid)
            {
                return ServiceResult<AnnouncementDTO>.Failure(validationResult);
            }

            var announcement = new Announcement
            {
                CourseId = request.CourseId,
                ProfessorId = request.ProfessorId,
                Title = request.Title,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Announcements.AddAsync(announcement);
            await _unitOfWork.SaveChangesAsync();

            var professor = await _unitOfWork.Users.GetByIdAsync(request.ProfessorId);

            var dto = new AnnouncementDTO
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Text = announcement.Text,
                Professor = professor?.Name ?? "Unknown",
                CreatedAt = announcement.CreatedAt
            };

            return ServiceResult<AnnouncementDTO>.Success(dto);
        }

        public async Task<IEnumerable<AnnouncementDTO>> GetByCourseAsync(int courseId)
        {
            var announcements = await _unitOfWork.Announcements.GetByCourseAsync(courseId);

            return announcements.Select(a => new AnnouncementDTO
            {
                Id = a.Id,
                Title = a.Title,
                Text = a.Text,
                Professor = a.Professor?.Name ?? "Unknown",
                CreatedAt = a.CreatedAt
            });
        }
    }
}