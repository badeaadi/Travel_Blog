using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.API.Exceptions;
using Identity.API.RequestObjects;
using Identity.Domain.Dtos;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public UserService(UserManager<ApiUser> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "Invalid credentials", "User with this email address already exists" }
                };
            }

            var newUser = new ApiUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ThumbnailUrl = request.ThumbnailUrl
            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = createdUser.Errors.Select(error => error.Description)
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var issuer = _jwtOptions.Issuer;
            var key = Encoding.ASCII.GetBytes(_jwtOptions.SigningKey);
            var tokenDescriptor = GenerateSecurityTokenDescriptor(newUser, issuer, key);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<AuthenticationResult> LoginAsync(UserLoginRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] {"Invalid credentials", "User does not exists" }
                };
            }

            if (!await _userManager.CheckPasswordAsync(existingUser, request.Password))
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "Invalid credentials", "User/password combination is incorrect" }
                };
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuer = _jwtOptions.Issuer;
            var key = Encoding.ASCII.GetBytes(_jwtOptions.SigningKey);
            var tokenDescriptor = GenerateSecurityTokenDescriptor(existingUser, issuer, key);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<ApiUserDto> GetUserByIdAsync(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                throw new UserNotFoundException("The requested user does not exist!");
            }

            return new ApiUserDto
            {
                Email = existingUser.Email,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                ThumbnailUrl = existingUser.ThumbnailUrl
            };
        }

        private static SecurityTokenDescriptor GenerateSecurityTokenDescriptor(ApiUser newUser, string issuer, byte[] key)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            return tokenDescriptor;
        }
    }
}