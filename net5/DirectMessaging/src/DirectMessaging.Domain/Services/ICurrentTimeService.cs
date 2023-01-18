namespace DirectMessaging.Domain.Services;

public interface ICurrentTimeService
{
    public DateTime CurrentTimeUtc();
}