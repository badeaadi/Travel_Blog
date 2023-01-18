using System;

namespace DirectMessaging.API.RequestObjects;

public class MessageCreationRequest
{
    public string Content { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}