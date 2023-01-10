using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace Identity.DataMigrator
{
    public enum DataMigrationResult
    {
        Success = 0,
        GenericError = -100
    }
    
    public class DataMigrator<T> 
        where T : DbContext
    {
        private readonly Func<T> _dbContextProvider;
        private readonly ILogger _logger;

        public DataMigrator(Func<T> dbContextProvider, ILogger logger)
        {
            _dbContextProvider = dbContextProvider;
            _logger = logger;
        }

        public async Task<DataMigrationResult> InstallAndMigrateDatabaseAsync()
        {
            await using var dbContext = _dbContextProvider();

            try
            {
                var dbMigrator = dbContext.Database.GetService<IMigrator>();
                await dbMigrator.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception: {Message}", ex.Message);
                return DataMigrationResult.GenericError;
            }

            return DataMigrationResult.Success;
        }

        public async Task<IEnumerable<string>> GetPendingMigrationsAsync()
        {
            await using var dbContext = _dbContextProvider();
            return await dbContext.Database.GetPendingMigrationsAsync();
        }
    }
}