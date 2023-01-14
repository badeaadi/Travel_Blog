using System.Threading.Tasks;
using Identity.API.Controllers;
using Identity.API.Services;
using Identity.Domain.Models;
using Identity.Infrastructure.Persistence.Contexts;
using Identity.Infrastructure.Persistence.Contexts.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            app.UseSwagger(opt => { opt.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("v1/swagger.json", "Identity Service V1");
                opt.EnableDeepLinking();
            });

            if (env.IsLocal())
            {
                Migrate(app);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureControllers(services);
            ConfigureInfrastructure(services);
            ConfigureContext(services);
            ConfigureHealthChecks(services);
            ConfigureIdentity(services);
            services.AddSwaggerGen(swaggerOption =>
            {
                swaggerOption.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service API", Version = "v1" });
                swaggerOption.DescribeAllParametersInCamelCase();
            });
        }

        private void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers(opt =>
                {
                    opt.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddApplicationPart(typeof(UserController).Assembly);
        }

        private void ConfigureInfrastructure(IServiceCollection services)
        {
            services.Configure<AppDbContextOptions>(opt => opt.Configure(Configuration));
            services.AddScoped<IDbContextProvider, DbContextProvider>();
            services.AddDbContext(Configuration);
        }

        private void ConfigureContext(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            services.Configure<JwtOptions>(Configuration.GetSection(nameof(JwtOptions)));
        }

        private void ConfigureHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services
                .AddIdentityCore<ApiUser>(opt => { opt.User.RequireUniqueEmail = true; })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
        }

        private async Task Migrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>() !.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<IDbContextProvider>();
            await context.GetOrCreateContext().Database.MigrateAsync();
        }
    }
}