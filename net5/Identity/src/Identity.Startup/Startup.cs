using Identity.Domain.Models;
using Identity.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.Startup
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("v1/swagger.json", "Identity Service V1");
                opt.EnableDeepLinking();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureControllers(services);
            ConfigureInfrastructure(services);
            ConfigureHealthChecks(services);
            ConfigureIdentity(services);
        }

        private void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                opt.AllowEmptyInputInBodyModelBinding = true;
            });
        }

        private void ConfigureInfrastructure(IServiceCollection services)
        {
            services.Configure<AppDbContextOptions>(opt => opt.Configure(Configuration));
            services.AddDbContext(Configuration);
        }

        private void ConfigureHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddAuthentication();
            
            services
                .AddIdentityCore<ApiUser>(opt => { opt.User.RequireUniqueEmail = true; })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthorization();
        }
    }
}