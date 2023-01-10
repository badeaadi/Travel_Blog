using System.Threading.Tasks;

namespace Identity.API.Services;

public interface ITokenService
{
    Task<TokenValidationResult> ValidateTokenAsync(string token);
}