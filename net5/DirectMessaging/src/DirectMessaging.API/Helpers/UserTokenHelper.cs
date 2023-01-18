using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DirectMessaging.API.Helpers;

public static class UserTokenHelper
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString == null)
        {
            throw new InvalidOperationException("User id not found!");
        }

        var parsed = Guid.TryParse(userIdString, out var userIdGuid);
        
        if (parsed == false)
        {
            throw new InvalidOperationException("User id not found!");
        }

        return userIdGuid;
    }
}