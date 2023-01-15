using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectMessaging.API.Helpers;
using DirectMessaging.API.RequestObjects;
using DirectMessaging.API.Services;
using DirectMessaging.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DirectMessaging.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class MessageController : Controller
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    
    [HttpGet]
    [Route("with/{otherUserId:guid}", Name = "Get messages between current user and another")]
    [Authorize]
    public async Task<IActionResult> GetMessagesFromUser([FromRoute] Guid otherUserId)
    {
      try
      {
          var currentUserId = UserTokenHelper.GetUserId(User);

          var messages = await _messageService.GetMessagesBetweenAsync(currentUserId, otherUserId);

          if (!messages.Any())
          {
              return NotFound($"No messages found between current user and {otherUserId}");
          }

          return Ok(messages);
      }
      catch (InvalidOperationException e)
      {
          return NotFound(e.Message);
      }
    }

    [HttpPost]
    [Route("to/{receiverId:guid}", Name = "Create a message from current user to receiver")]
    [Authorize]
    public async Task<IActionResult> CreateMessage([FromRoute] Guid receiverId, [FromBody] MessageCreationRequest request)
    {
        try
        {
            var senderId = UserTokenHelper.GetUserId(User);
    
            var newMessage = await _messageService.CreateMessageAsync(senderId, receiverId, request);
    
            return Ok(newMessage);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
}