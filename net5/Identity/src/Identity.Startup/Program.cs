using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.Startup
{
    public static class Program
    {
        private const string SecretsPath = "/config/kubernetes-secrets";
        
        public static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());
            var _logger = loggerFactory.CreateLogger("Program");

            try
            {
                _logger.LogInformation("Identity Service is starting...");
                Console.Title = "Identity";
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                loggerFactory.Dispose();
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        if (hostingContext.HostingEnvironment.IsProduction())
                        {
                            config.AddKeyPerFile(SecretsPath, false);
                        }
                    })
                    .UseStartup<Startup>()
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    });
            });
    }
}