using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DirectMessaging.API.RequestObjects;
using DirectMessaging.Domain.Dtos;
using DirectMessaging.Domain.Repositories;

namespace DirectMessaging.API.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;
    private readonly IMapper _mapper;

    public MessageService(IMessageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesBetweenAsync(Guid currentUserId, Guid otherUserId)
    {
        var messagesFrom = await _repository.GetMessagesAsync(otherUserId, currentUserId);
        var messagesTo = await _repository.GetMessagesAsync(currentUserId, otherUserId);

        return _mapper.Map<IEnumerable<MessageDto>>(messagesFrom.Concat(messagesTo));
    }

    public async Task<MessageDto> CreateMessageAsync(Guid senderId, Guid receiverId, MessageCreationRequest request)
    {
        var message = await _repository.CreateMessageAsync(senderId, receiverId, request.Content, request.CreatedAt);
        
        return _mapper.Map<MessageDto>(message);
    }
}