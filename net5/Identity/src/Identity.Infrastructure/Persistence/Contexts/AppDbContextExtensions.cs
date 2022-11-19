using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Persistence.Contexts
{
    public static class AppDbContextExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.Configure(GetDbOptionsConfig(configuration)));
        }

        public static DbContextOptionsBuilder Configure(this DbContextOptionsBuilder builder, AppDbContextOptions options)
        {
            builder.UseMySQL(options.ConnectionString);
            return builder;
        }
        
        public static AppDbContextOptions Configure(this AppDbContextOptions appDbContextOptions, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection("Database");
            appDbContextOptions.ConnectionString = configurationSection[nameof(appDbContextOptions.ConnectionString)];
            
            return appDbContextOptions;
        }

        private static AppDbContextOptions GetDbOptionsConfig(IConfiguration configuration)
        {
            return Configure(new AppDbContextOptions(), configuration);
        }
    }
}