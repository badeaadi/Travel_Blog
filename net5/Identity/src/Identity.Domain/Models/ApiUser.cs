using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Models
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string ThumbnailUrl { get; set; }
    }
}