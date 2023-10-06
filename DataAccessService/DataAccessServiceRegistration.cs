using DataAccessService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLibrary.Persistence;
using SimpleMediaLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessService
{
    public static class DataAccessServiceRegistration
    {
        public static void AddDataAccesServices(this IServiceCollection services,
                                                IConfiguration configuration)
        {
            // Reading database path from appsettings.json
            string dbPath = configuration.GetConnectionString("SimpleLibraryDatabase")
                ?? throw new KeyNotFoundException("Unable to find `SimpleLibraryDatabase` connection string in " +
                "appsettings.json or other configuration.");

            services.AddDbContext<SimpleLibraryDbContext>(options =>
                options.UseSqlite(dbPath));

            services.AddSingleton<MediaFileEventMediator>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            services.AddScoped<IAppConfigRepository, AppConfigRepository>();
        }
    }
}
