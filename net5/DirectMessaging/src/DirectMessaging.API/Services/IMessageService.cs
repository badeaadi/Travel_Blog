using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DirectMessaging.API.RequestObjects;
using DirectMessaging.Domain.Dtos;

namespace DirectMessaging.API.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageDto>> GetMessagesBetweenAsync(Guid currentUserId, Guid otherUserId);
    Task<MessageDto> CreateMessageAsync(Guid senderId, Guid receiverId, MessageCreationRequest request);
}