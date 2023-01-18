using DirectMessaging.Infrastructure.Persistence.Contexts;
using DirectMessaging.Infrastructure.Persistence.Contexts.Provider;

namespace DirectMessaging.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected AppDbContext Context { get; }

        protected BaseRepository(IDbContextProvider dbContextProvider)
        {
            Context = dbContextProvider.GetOrCreateContext();
        }
    }
}