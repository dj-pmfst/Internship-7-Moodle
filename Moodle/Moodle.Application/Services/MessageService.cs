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

        public async Task<IEnumerable<MessageDTO>> GetConversationAsync(int userId1, int userId2)
        {
            var messages = await _unitOfWork.Messages.GetConversationMessagesAsync(userId1, userId2);
            var currentUserId = userId1;

            var otherUserId = userId1 == currentUserId ? userId2 : userId1;
            var otherUser = await _unitOfWork.Users.GetByIdIncludingDeletedAsync(otherUserId);
            bool isOtherUserDeleted = (otherUser == null || otherUser.IsDeleted);

            var result = new List<MessageDTO>();

            foreach (var message in messages)
            {
                bool isSentByCurrentUser = message.SenderId == currentUserId;

                result.Add(new MessageDTO
                {
                    Id = message.Id,
                    SenderId = message.SenderId ?? 0,        
                    ReceiverId = message.ReceiverId ?? 0,   
                    Text = (!isSentByCurrentUser && isOtherUserDeleted)
                        ? "[Izbrisana poruka]"
                        : message.Text,
                    SenderName = "",
                    ReceiverName = "", 
                    SentAt = message.SentAt,
                    IsSentByCurrentUser = isSentByCurrentUser
                });
            }

            return result;
        }

        public async Task<IEnumerable<UserDTO>> GetConversationPartnersAsync(int userId)
        {
            var partners = await _unitOfWork.Messages.GetConversationPartnersAsync(userId);

            var result = new List<UserDTO>();

            foreach (var user in partners)
            {
                if (user == null || user.IsDeleted)
                {
                    result.Add(new UserDTO
                    {
                        Id = user?.Id ?? -1,
                        Name = "[Izbrisan korisnik]",
                        Email = "",
                        Role = Domain.Enums.Roles.student,
                        IsActive = false
                    });
                }
                else
                {
                    result.Add(new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        IsActive = user.IsActive
                    });
                }
            }
            return result;
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