using Moodle.Application.DTOs.User;
using Moodle.Application.Interfaces;
using Moodle.Domain.Entities;
using Moodle.Domain.Enums;
using Moodle.Moodle.Application.Common;
using Moodle.Moodle.Application.Validators.Format;

namespace Moodle.Application.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDTO>> GetAllStudentsAsync()
        {
            var students = await _unitOfWork.Users.GetAllStudentsAsync();
            return students.Select(s => MapToDto(s));
        }

        public async Task<IEnumerable<UserDTO>> GetAllProfessorsAsync()
        {
            var professors = await _unitOfWork.Users.GetAllProfessorsAsync();
            return professors.Select(p => MapToDto(p));
        }

        public async Task<IEnumerable<UserDTO>> GetByRoleAsync(Roles role)
        {
            var users = await _unitOfWork.Users.GetByRoleAsync(role);
            return users.Select(u => MapToDto(u));
        }

        public async Task<UserDTO?> GetByIdAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<bool>.Failure("Korisnik nije pronađen.");
            }

            if (user.IsAdmin())
            {
                return ServiceResult<bool>.Failure("Admin se ne može obrisati");
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<UserDTO>> UpdateEmailAsync(UpdateUserEmailRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return ServiceResult<UserDTO>.Failure("Korisnik nije pronađen.");
            }

            var emailValidation = ValidatorEmail.Validate(request.NewEmail);
            if (!emailValidation.IsValid)
            {
                return ServiceResult<UserDTO>.Failure(emailValidation);
            }

            if (await _unitOfWork.Users.EmailExistsAsync(request.NewEmail, request.UserId))
            {
                return ServiceResult<UserDTO>.Failure("Email već postoji");
            }

            user.Email = request.NewEmail;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<UserDTO>.Success(MapToDto(user));
        }

        public async Task<ServiceResult<UserDTO>> ChangeRoleAsync(ChangeRoleRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return ServiceResult<UserDTO>.Failure("Korisnik nije pronađen");
            }

            if (user.IsAdmin())
            {
                return ServiceResult<UserDTO>.Failure("Admin se ne može urediti");
            }

            if (user.IsStudent())
            {
                user.PromoteToProfessor();
            }
            else if (user.Role == Roles.profesor)
            {
                user.DemoteToStudent();
            }

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<UserDTO>.Success(MapToDto(user));
        }

        private UserDTO MapToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }
    }
}