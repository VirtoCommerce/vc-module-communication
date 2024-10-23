using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VirtoCommerce.CommunicationModule.Data.Repositories;

namespace VirtoCommerce.CommunicationModule.Data.SqlServer;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommunicationDbContext>
{
    public CommunicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<CommunicationDbContext>();
        var connectionString = args.Length != 0 ? args[0] : "Server=(local);User=virto;Password=virto;Database=VirtoCommerce3;";

        builder.UseSqlServer(
            connectionString,
            options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

        return new CommunicationDbContext(builder.Options);
    }
}
