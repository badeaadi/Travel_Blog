using System;
using DirectMessaging.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DirectMessaging.Infrastructure.Persistence.Contexts.Provider
{
    public class DbContextProvider: IDbContextProvider, IDisposable
    {
        private readonly AppDbContextOptions _options;
        private readonly ICurrentTimeService _currentTimeService;
        private AppDbContext _context;

        private bool _contextDisposed;

        public DbContextProvider(IOptions<AppDbContextOptions> options, ICurrentTimeService currentTimeService)
        {
            _options = options.Value;
            _currentTimeService = currentTimeService;
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

            _context = new AppDbContext(builder.Options, _currentTimeService);

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