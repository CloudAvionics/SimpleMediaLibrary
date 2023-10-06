using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using DataAccessService.DataAccess;
using Persistence.Model;

public class FileService : IHostedService, IDisposable
{
    private readonly ILogger<FileService> _logger;
    private readonly IAppConfigRepository _appConfig;
    private readonly IMediaFileRepository _mediaFileRepository;
    private FileSystemWatcher _watcher;

    public FileService(ILogger<FileService> logger, IServiceProvider serviceProvider,
                       IMediaFileRepository mediaFileRepository, IAppConfigRepository appConfig)
    {
        _logger = logger;
        _mediaFileRepository = mediaFileRepository;
        _appConfig = appConfig;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _watcher = new FileSystemWatcher();
        _watcher.Path = await _appConfig.GetRecordingPathConfigAsync();
        _watcher.Filter = "*.mp3";

        _watcher.Created += FileCreated;

        _watcher.EnableRaisingEvents = true;

        return;
    }

    private async void FileCreated(object sender, FileSystemEventArgs e)
    {
        FileInfo fileInfo = new FileInfo(e.FullPath);

        try
        {
            var mediaFile = new MediaFile
            {
                Name = fileInfo.Name,
                Time = fileInfo.CreationTime,
                FileSize = fileInfo.Length
            };

            await _mediaFileRepository.AddMediaFileAsync(mediaFile);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to insert new file metadata: ", ex);
        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Dispose();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _watcher?.Dispose();
    }
}
