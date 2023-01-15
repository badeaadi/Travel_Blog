namespace DirectMessaging.Domain.Dtos;

public class MessageDto
{
    public string Content { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public Guid SenderId { get; set; }
    
    public Guid ReceiverId { get; set; }
}