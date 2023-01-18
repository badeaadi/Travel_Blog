using System;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper.EquivalencyExpression;
using DirectMessaging.API.Controllers;
using DirectMessaging.API.Services;
using DirectMessaging.Domain.Mapping;
using DirectMessaging.Domain.Repositories;
using DirectMessaging.Domain.Services;
using DirectMessaging.Infrastructure.Persistence.Contexts;
using DirectMessaging.Infrastructure.Persistence.Contexts.Provider;
using DirectMessaging.Infrastructure.Repositories;
using DirectMessaging.Infrastructure.Services;
using DirectMessaging.Startup.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DirectMessaging.Startup
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger(opt => { opt.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("v1/swagger.json", "Direct Messaging Service V1");
                opt.EnableDeepLinking();
            });

            if (env.IsLocal())
            {
                Migrate(app);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMappings(services);
            ConfigureControllers(services);
            ConfigureInfrastructure(services);
            ConfigureContext(services);
            ConfigureHealthChecks(services);
            ConfigureAuthentication(services);
            services.AddSwaggerGen(swaggerOption =>
            {
                swaggerOption.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Name = "bearerAuth",
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });

                swaggerOption.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }, },
                        Array.Empty<string>()
                    }
                });

                swaggerOption.SwaggerDoc("v1", new OpenApiInfo { Title = "Direct Messaging Service API", Version = "v1" });
                swaggerOption.DescribeAllParametersInCamelCase();
            });
        }

        private void ConfigureMappings(IServiceCollection services)
        {
            services.AddAutoMapper(
                cfg => { cfg.AddCollectionMappers(); },
                Assembly.GetExecutingAssembly(),
                typeof(DtoMappingProfile).Assembly
            );
        }

        private void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers(opt => { opt.AllowEmptyInputInBodyModelBinding = true; })
                .AddApplicationPart(typeof(MessageController).Assembly);
        }

        private void ConfigureInfrastructure(IServiceCollection services)
        {
            services.Configure<AppDbContextOptions>(opt => opt.Configure(Configuration));
            services.AddScoped<IDbContextProvider, DbContextProvider>();
            services.AddDbContext(Configuration);

            services.AddScoped<IMessageRepository, MessageRepository>()
                .AddSingleton<ICurrentTimeService, CurrentTimeService>();
        }

        private void ConfigureContext(IServiceCollection services)
        {
            services.AddScoped<IMessageService, MessageService>();
        }

        private void ConfigureHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddIdentityAuthorization(Configuration);
        }

        private async Task Migrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>() !.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<IDbContextProvider>();
            await context.GetOrCreateContext().Database.MigrateAsync();
        }
    }
}