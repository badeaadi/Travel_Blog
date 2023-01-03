using System.Collections.Generic;

namespace Identity.API.ResponseObjects
{
    public class AuthenticationFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}