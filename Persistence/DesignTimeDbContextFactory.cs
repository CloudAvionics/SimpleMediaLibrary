using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SimpleLibrary.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SimpleLibraryDbContext>
    {
        public SimpleLibraryDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<SimpleLibraryDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlite(connectionString);

            return new SimpleLibraryDbContext(builder.Options);
        }
    }
}
