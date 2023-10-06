using DataAccessService.DataAccess;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class StartupService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;   
    public StartupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

        using (var scope = _scopeFactory.CreateScope())
        {
            var mediaFileRepository = scope.ServiceProvider.GetRequiredService<IMediaFileRepository>();
            var configRepository = scope.ServiceProvider.GetRequiredService<IAppConfigRepository>();

            await SyncFilesWithDatabase(mediaFileRepository, configRepository);
        }

        return;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Cleanup code if needed
        return Task.CompletedTask;
    }

    private async Task SyncFilesWithDatabase(IMediaFileRepository mediaFileRepository,
                                             IAppConfigRepository configRepository)
    {
        // Get all the MediaFiles from database
        var mediaFilesFromDb = await mediaFileRepository.GetMediaFiles();

        // Assuming you have a method that returns the directory path
        string directoryPath = await GetMediaFilesDirectoryPath(configRepository);

        // Get all the actual files from the disk
        var actualFiles = Directory.GetFiles(directoryPath, "*.mp3").Select(f => Path.GetFileName(f));

        // Identify the files that are in database but not on disk
        var filesToRemove = mediaFilesFromDb.Where(dbFile => !actualFiles.Contains(dbFile.Name)).ToList();

        // Remove these entries from the database
        foreach (var fileToRemove in filesToRemove)
        {
            await mediaFileRepository.RemoveMediaFileAsync(fileToRemove);
        }
    }

    private async Task<string> GetMediaFilesDirectoryPath(IAppConfigRepository configRepository)
    {
        return await configRepository.GetRecordingPathConfigAsync();
    }
}
