using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProPlan.Repositories.Contracts;

namespace ProPlan.WebApi.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<RepositoryContext>().
                UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                options => options.MigrationsAssembly("ProPlan.WebApi"));

            return new RepositoryContext(builder.Options);
        }
    }
}
