using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DirectMessaging.Startup
{
    public static class Program
    {
        private const string SecretsPath = "/config/kubernetes-secrets";
        
        public static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());
            var logger = loggerFactory.CreateLogger("Program") ?? throw new ArgumentNullException("loggerFactory.CreateLogger(\"Program\")");

            try
            {
                logger.LogInformation("Direct Messaging Service is starting...");
                Console.Title = "Direct Messaging";
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Stopped program because of exception");
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