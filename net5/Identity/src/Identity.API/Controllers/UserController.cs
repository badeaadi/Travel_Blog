using System.Linq;
using System.Threading.Tasks;
using Identity.API.RequestObjects;
using Identity.API.ResponseObjects;
using Identity.API.ResultObjects.ResponseObjects;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register", Name = "Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(entry => entry.Errors.Select(error => error.ErrorMessage))
                });
            }
            
            var authenticationResponse = await _userService.RegisterAsync(request);

            if (!authenticationResponse.Success)
            {
                return BadRequest(new AuthenticationFailedResponse
                {
                    Errors = authenticationResponse.Errors
                });
            }
            
            return Ok(new AuthenticationSuccessResponse
            {
                Token = authenticationResponse.Token
            });
        }
        
        [HttpPost]
        [Route("login", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authenticationResponse = await _userService.LoginAsync(request);

            if (!authenticationResponse.Success)
            {
                return BadRequest(new AuthenticationFailedResponse
                {
                    Errors = authenticationResponse.Errors
                });
            }
            
            return Ok(new AuthenticationSuccessResponse
            {
                Token = authenticationResponse.Token
            });
        }
    }
}