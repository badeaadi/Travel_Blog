using System.Threading.Tasks;
using Identity.API.RequestObjects;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("validate", Name = "Validate")]
        public async Task<IActionResult> Validate([FromBody] TokenValidationRequest request)
        {
            var validationResponse = await _tokenService.ValidateTokenAsync(request.Token);

            if (!validationResponse.IsValid)
            {
                return BadRequest(validationResponse.Exception.Message);
            }

            return Ok();
        }
    }
}