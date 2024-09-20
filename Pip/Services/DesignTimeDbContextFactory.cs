using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pip.DataAccess;

namespace Pip.UI.Services;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PipDbContext>
{
	public PipDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<PipDbContext>();
		//var connectionString = ConfigurationManager.ConnectionStrings["PipDbLocal"].ConnectionString;
		//optionsBuilder.UseSqlServer(connectionString, ob => ob.MigrationsAssembly("Pip.DataAccess"));
		optionsBuilder.UseSqlite("Data Source=pip.db", ob => ob.MigrationsAssembly("Pip.DataAccess"));
		return new PipDbContext(optionsBuilder.Options);
	}
}
