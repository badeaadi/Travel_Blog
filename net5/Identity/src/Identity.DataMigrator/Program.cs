using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Identity.DataMigrator.Options;
using Identity.Infrastructure.Persistence.Contexts;
using Identity.Infrastructure.Persistence.Contexts.Provider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.DataMigrator
{
    public static class Program
    {
        private static ILogger _logger = null!;

        public static async Task<int> Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());
            _logger = loggerFactory.CreateLogger("DataMigrator");

            var task = Parser.Default.ParseArguments<DataMigratorOptions>(args).MapResult(
                Migrate,
                HandleParseError
            );

            return await task;
        }

        private static async Task<int> Migrate(DataMigratorOptions options)
        {
            var opts = new OptionsWrapper<AppDbContextOptions>(new AppDbContextOptions
            {
                ConnectionString = options.ConnectionString
            });

            var migrator = new DataMigrator<AppDbContext>(() => new DbContextProvider(opts).GetOrCreateContext(), _logger);

            await Console.Out.WriteLineAsync("Database schema migration for Identity");
            var pendingMigrations = (await migrator.GetPendingMigrationsAsync()).ToList();
            await Console.Out.WriteLineAsync($"Pending migrations count={pendingMigrations.Count}");
            foreach (var migration in pendingMigrations)
            {
                await Console.Out.WriteLineAsync($"    - {migration}");
            }

            var migrationResult = await migrator.InstallAndMigrateDatabaseAsync();

            if (migrationResult == DataMigrationResult.Success)
            {
                await Console.Out.WriteLineAsync($"Done database schema migration for Identity.");
            }

            return (int)migrationResult;
        }

        private static Task<int> HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                _logger.LogError(error.ToString());
            }

            return Task.FromResult(-1);
        }
    }
}