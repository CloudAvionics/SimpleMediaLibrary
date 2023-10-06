using Microsoft.EntityFrameworkCore;
using Persistence.Model;
using System;

namespace SimpleLibrary.Persistence
{
    public class SimpleLibraryDbContext: DbContext
    {
        public SimpleLibraryDbContext(DbContextOptions<SimpleLibraryDbContext> options) : base(options)
        {
        }

        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<MediaFileMetadata> MediaFilesMetadatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(new AppConfig
            {
                Id = 1,
                ConfigName = "RecordingDir",
                ConfigValue = Path.Combine("Recordings")
            },
            new AppConfig{
                Id = 2,
                ConfigName = "ApplicationName",
                ConfigValue = "Simple Media Library"
            },
            new AppConfig
            {
                Id = 3,
                ConfigName = "CompanyName",
                ConfigValue = "CloudAvionics"
            },
            new AppConfig
            {
                Id = 4,
                ConfigName = "MediaFileNamingConvention",
                ConfigValue = "{station}_{genre}_{title}_{publishdate}_{publishtime}"
            }
            );
        }
    }
}
