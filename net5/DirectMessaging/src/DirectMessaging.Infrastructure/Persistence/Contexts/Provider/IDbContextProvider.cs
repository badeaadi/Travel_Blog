namespace DirectMessaging.Infrastructure.Persistence.Contexts.Provider
{
    public interface IDbContextProvider
    {
        AppDbContext GetOrCreateContext();
    }
}