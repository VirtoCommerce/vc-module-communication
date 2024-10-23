using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.CommunicationModule.Data.Repositories;

namespace VirtoCommerce.CommunicationModule.Data.PostgreSql;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommunicationDbContext>
{
    public CommunicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<CommunicationDbContext>();
        var connectionString = args.Any() ? args[0] : "Server=localhost;Username=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

        return new CommunicationDbContext(builder.Options);
    }
}
