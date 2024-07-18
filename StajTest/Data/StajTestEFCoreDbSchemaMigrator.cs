using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace StajTest.Data;

public class StajTestEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public StajTestEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the StajTestDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<StajTestDbContext>()
            .Database
            .MigrateAsync();
    }
}
