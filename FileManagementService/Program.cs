using DataAccessService;
using DataAccessService.DataAccess;
using Persistence.Model;
using SimpleMediaLibrary.FileManagementService;

namespace FileManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddHostedService<StartupService>();
                    services.AddHostedService<Worker>();
                    services.AddDataAccesServices(configuration);

                    // other registrations
                });
    }

}