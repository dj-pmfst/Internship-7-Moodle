using Moodle.Application.DTOs.Material;
using Moodle.Application.Interfaces;
using Moodle.Application.Validators.Format;
using Moodle.Domain.Entities;
using Moodle.Moodle.Application.Common;
using Moodle.Moodle.Application.Validators.Format;
using ValidationResult = Moodle.Domain.Common.Validations.ValidationResult;

namespace Moodle.Application.Services
{
    public class MaterialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<MaterialDTO>> AddMaterialAsync(AddMaterialRequest request)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                validationResult.AddError("NameRequired", "Namerequired");
            }

            var urlValidation = UrlValidator.Validate(request.Url);
            if (!urlValidation.IsValid)
            {
                MergeValidationResults(validationResult, urlValidation);
            }

            if (!validationResult.IsValid)
            {
                return ServiceResult<MaterialDTO>.Failure(validationResult);
            }

            var material = new Material
            {
                CourseID = request.CourseId,
                Name = request.Name,
                Url = request.Url,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Materials.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();

            var dto = new MaterialDTO
            {
                Id = material.Id,
                Name = material.Name,
                Url = material.Url,
                CreatedAt = material.CreatedAt
            };

            return ServiceResult<MaterialDTO>.Success(dto);
        }

        public async Task<IEnumerable<MaterialDTO>> GetByCourseAsync(int courseId)
        {
            var materials = await _unitOfWork.Materials.GetByCourseAsync(courseId);

            return materials.Select(m => new MaterialDTO
            {
                Id = m.Id,
                Name = m.Name,
                Url = m.Url,
                CreatedAt = m.CreatedAt
            });
        }

        private void MergeValidationResults(ValidationResult target, ValidationResult source)
        {
            foreach (var item in source.ValidationItems)
            {
                target.AddValidationItem(item);
            }
        }
    }
}