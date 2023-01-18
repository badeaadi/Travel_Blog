using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectMessaging.Domain.Models;
using DirectMessaging.Domain.Repositories;
using DirectMessaging.Infrastructure.Persistence.Contexts.Provider;
using Microsoft.EntityFrameworkCore;

namespace DirectMessaging.Infrastructure.Repositories;

public class MessageRepository : BaseRepository, IMessageRepository
{
    public MessageRepository(IDbContextProvider dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(Guid senderId, Guid receiverId)
    {
        return await Context.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
            .ToListAsync();
    }

    public async Task<Message> CreateMessageAsync(Guid senderId, Guid receiverId, string content, DateTime? createdAt)
    {
        var message = await Context.Messages.AddAsync(new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            CreatedAt = createdAt
        });

        await Context.SaveChangesAsync();

        return message.Entity;
    }
}