using System.ComponentModel.DataAnnotations;

namespace DirectMessaging.Domain.Models;

public class Message : ICreatedAt
{
    [Key]
    public Guid Id { get; set; }
    
    [StringLength(1000)]
    public string Content { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public Guid SenderId { get; set; }
    
    public Guid ReceiverId { get; set; }
}