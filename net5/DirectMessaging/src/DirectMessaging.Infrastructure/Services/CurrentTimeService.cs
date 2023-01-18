using System;
using DirectMessaging.Domain.Services;

namespace DirectMessaging.Infrastructure.Services;

public class CurrentTimeService : ICurrentTimeService
{
    public DateTime CurrentTimeUtc()
    {
        return DateTime.Now.ToUniversalTime();
    }
}