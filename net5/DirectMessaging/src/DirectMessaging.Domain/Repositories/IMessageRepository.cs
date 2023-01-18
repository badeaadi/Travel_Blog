using DirectMessaging.Domain.Models;

namespace DirectMessaging.Domain.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetMessagesAsync(Guid senderId, Guid receiverId);
    Task<Message> CreateMessageAsync(Guid senderId, Guid receiverId, string content, DateTime? createdAt);
}