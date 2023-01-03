using System.Threading.Tasks;
using Identity.API.RequestObjects;
using Identity.API.ResultObjects;

namespace Identity.API.Services
{
    public interface IUserService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(UserLoginRequest request);
    }
}