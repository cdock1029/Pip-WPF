using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pip.DataAccess;

namespace Pip.UI.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PipDbContext>
{
    public PipDbContext CreateDbContext(string[] args)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["PipDbLocal"].ConnectionString;
        var optionsBuilder = new DbContextOptionsBuilder<PipDbContext>();
        optionsBuilder.UseSqlServer(connectionString, ob => ob.MigrationsAssembly("Pip.DataAccess"));
        return new PipDbContext(optionsBuilder.Options);
    }
}
