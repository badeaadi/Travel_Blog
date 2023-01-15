using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DirectMessaging.Startup.Identity;

public class IdentityTokenValidator : ISecurityTokenValidator
{
    private readonly SecurityTokenHandler _tokenHandler;

    private readonly string _authority;

    private readonly byte[] _key;

    public IdentityTokenValidator(string authority, string keyAsString)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _authority = authority;
        _key = Encoding.ASCII.GetBytes(keyAsString);
    }

    public bool CanValidateToken { get; } = true;

    public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;


    public bool CanReadToken(string securityToken)
    {
        if (!_tokenHandler.CanReadToken(securityToken))
        {
            return false;
        }

        SecurityToken parsedToken;

        try
        {
            parsedToken = _tokenHandler.ReadToken(securityToken);
        }
        catch (ArgumentException)
        {
            return false;
        }

        return parsedToken != null;
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        validationParameters.ValidIssuer = _authority;
        validationParameters.IssuerSigningKey = new SymmetricSecurityKey(_key);

        return _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
    }
}