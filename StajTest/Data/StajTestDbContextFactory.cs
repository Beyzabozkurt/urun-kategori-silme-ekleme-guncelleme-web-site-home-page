using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StajTest.Data;

public class StajTestDbContextFactory : IDesignTimeDbContextFactory<StajTestDbContext>
{
    public StajTestDbContext CreateDbContext(string[] args)
    {

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<StajTestDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new StajTestDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
