using CommandLine;

namespace Identity.DataMigrator.Options
{
    public class DataMigratorOptions
    {
        [Option('c', "connectionString", Required = true, HelpText = "Connection string of the database to create or migrate.")]
        public string? ConnectionString { get; set; }
    }
}