using Identity.Infrastructure.Persistence.Contexts;
using Identity.Infrastructure.Persistence.Contexts.Provider;

namespace Identity.Infrastructure.Repositories
{
    public class BaseRepository
    {
        protected AppDbContext Context { get; }
        
        protected BaseRepository(IDbContextProvider dbContextProvider)
        {
            Context = dbContextProvider.GetOrCreateContext();
        }
    }
}