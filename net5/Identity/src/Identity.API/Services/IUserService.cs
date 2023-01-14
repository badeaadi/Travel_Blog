using System.Threading.Tasks;
using Identity.API.RequestObjects;
using Identity.Domain.Dtos;

namespace Identity.API.Services
{
    public interface IUserService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(UserLoginRequest request);
        Task<ApiUserDto> GetUserByIdAsync(string userId);
    }
}