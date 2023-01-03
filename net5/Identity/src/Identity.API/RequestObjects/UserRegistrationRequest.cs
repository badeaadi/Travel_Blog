using System.ComponentModel.DataAnnotations;

namespace Identity.API.RequestObjects
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}