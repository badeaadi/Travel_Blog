using System.ComponentModel.DataAnnotations;

namespace Identity.API.RequestObjects
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}