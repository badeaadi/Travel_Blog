using System.Threading;
using System.Threading.Tasks;
using DirectMessaging.Domain.Models;
using DirectMessaging.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace DirectMessaging.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentTimeService _currentTimeService;
        
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentTimeService currentTimeService)
            : base(options)
        {
            _currentTimeService = currentTimeService;
        }

        public DbSet<Message> Messages { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
        {
            var now = _currentTimeService.CurrentTimeUtc();

            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is ICreatedAt createdAtEntity)
                        {
                            createdAtEntity.CreatedAt ??= now;
                        }
                        break;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("dm");

            base.OnModelCreating(builder);
        }
    }
}