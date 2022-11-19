using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Identity.Startup
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsLocal(this IWebHostEnvironment env)
        {
            return env.IsEnvironment("Local");
        }
    }
}