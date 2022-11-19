using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Persistence.Contexts.Provider
{
    public class DbContextProvider: IDbContextProvider, IDisposable
    {
        private readonly AppDbContextOptions _options;
        private AppDbContext _context;

        private bool _contextDisposed;

        public DbContextProvider(IOptions<AppDbContextOptions> options)
        {
            _options = options.Value;
        }
        
        public void Dispose()
        {
            DisposeContext();
            GC.SuppressFinalize(this);
        }

        public AppDbContext GetOrCreateContext()
        {
            if (_context != null)
            {
                return _context;
            }

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.Configure(_options);

            _context = new AppDbContext(builder.Options);

            return _context;
        }

        private void DisposeContext()
        {
            if (_contextDisposed)
            {
                return;
            }

            _context.Dispose();
            _contextDisposed = true;
        }
    }
}