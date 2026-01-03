using Moodle.Application.DTOs.Auth;
using Moodle.Application.Interfaces;
using Moodle.Application.Validators.Format;
using Moodle.Domain.Entities;
using Moodle.Domain.Enums;
using Moodle.Moodle.Application.Common;
using Moodle.Moodle.Application.Validators.Format;
using ValidationResult = Moodle.Domain.Common.Validations.ValidationResult;  

namespace Moodle.Application.Services
{
    public class AuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

            if (user == null || request.Password != user.Password)
            {
                return ServiceResult<LoginResponse>.Failure("Neispravan mail ili šifra");
            }

            if (!user.IsActive)
            {
                return ServiceResult<LoginResponse>.Failure("Račun je deaktiviran.");
            }

            var response = new LoginResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return ServiceResult<LoginResponse>.Success(response);
        }

        public async Task<ServiceResult<LoginResponse>> RegisterAsync(RegisterRequest request)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                validationResult.AddError("NameRequired", "Name required");
            }

            var emailValidation = ValidatorEmail.Validate(request.Email);
            if (!emailValidation.IsValid)
            {
                MergeValidationResults(validationResult, emailValidation);
            }

            var passwordValidation = ValidationPassword.Validate(request.Password);
            if (!passwordValidation.IsValid)
            {
                MergeValidationResults(validationResult, passwordValidation);
            }

            if (request.Password != request.ConfirmPassword)
            {
                validationResult.AddError("PasswordMismatch", "Šifre se ne slažu");
            }

            if (emailValidation.IsValid && await _unitOfWork.Users.EmailExistsAsync(request.Email))
            {
                validationResult.AddError("EmailExists", "\nEmail već postoji");
            }

            if (!validationResult.IsValid)
            {
                return ServiceResult<LoginResponse>.Failure(validationResult);
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                Role = Roles.student,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var response = new LoginResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return ServiceResult<LoginResponse>.Success(response);
        }

        public string GenerateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var captcha = new char[6];

            captcha[0] = chars[random.Next(0, 52)];
            captcha[1] = chars[random.Next(52, 62)];

            for (int i = 2; i < 6; i++)
            {
                captcha[i] = chars[random.Next(chars.Length)];
            }

            return new string(captcha.OrderBy(x => random.Next()).ToArray());
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