using Moodle.Application.DTOs.Message;
using Moodle.Application.DTOs.User;
using Moodle.Application.Interfaces;
using Moodle.Domain.Entities;
using Moodle.Moodle.Application.Common;

namespace Moodle.Application.Services
{
    public class MessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<MessageDTO>> SendMessageAsync(SendMessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return ServiceResult<MessageDTO>.Failure("Poruka ne može biti prazna.");
            }

            var message = new Message
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Text = request.Text,
                SentAt = DateTime.UtcNow
            };

            await _unitOfWork.Messages.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            var sender = await _unitOfWork.Users.GetByIdAsync(request.SenderId);
            var receiver = await _unitOfWork.Users.GetByIdAsync(request.ReceiverId);

            var dto = new MessageDTO
            {
                Id = message.Id,
                Text = message.Text,
                SenderName = sender?.Name ?? "Unknown",
                ReceiverName = receiver?.Name ?? "Unknown",
                SentAt = message.SentAt
            };

            return ServiceResult<MessageDTO>.Success(dto);
        }

        public async Task<IEnumerable<MessageDTO>> GetConversationAsync(int userId1, int userId2, int currentUserId)
        {
            var messages = await _unitOfWork.Messages.GetConversationAsync(userId1, userId2);

            return messages.Select(m => new MessageDTO
            {
                Id = m.Id,
                Text = m.Text,
                SenderName = m.Sender?.Name ?? "Unknown",
                ReceiverName = m.Receiver?.Name ?? "Unknown",
                SentAt = m.SentAt,
                IsSentByCurrentUser = m.SenderId == currentUserId
            });
        }

        public async Task<IEnumerable<UserDTO>> GetConversationPartnersAsync(int userId)
        {
            var partners = await _unitOfWork.Messages.GetConversationPartnersAsync(userId);

            return partners.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive
            });
        }

        public async Task<IEnumerable<UserDTO>> GetUsersWithoutConversationAsync(int userId)
        {
            var users = await _unitOfWork.Messages.GetUsersWithoutConversationAsync(userId);

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive
            });
        }
    }
}