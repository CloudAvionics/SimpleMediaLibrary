using DataAccessService.DataAccess;
using Persistence.Model;
using SimpleMediaLibrary.Common;
using System.IO;

namespace Player.Services
{
    public class FileManagementBackgroundService : BackgroundService
    {
        private readonly ILogger<FileManagementBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileSyncService _fileSyncService;
        private readonly IMediaFileEventMediator _mediator;
        private readonly IMediaFileMetaDataService _fileMetaDataService;

        string libraryFilePath = String.Empty;

        public FileManagementBackgroundService(ILogger<FileManagementBackgroundService> logger,
            IServiceProvider serviceProvider,
            IFileSyncService fileSyncService,
            IMediaFileEventMediator mediator,
            IMediaFileMetaDataService fileMetaDataService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _fileSyncService = fileSyncService;
            _mediator = mediator;
            _fileMetaDataService = fileMetaDataService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Sync files with database befor we continue
                await _fileSyncService.TaskSyncFilesWithDatabaseAsync();

                using (var initialScope = _serviceProvider.CreateScope())
                {
                    var appConfig = initialScope.ServiceProvider.GetRequiredService<IAppConfigRepository>();
                    libraryFilePath = await appConfig.GetRecordingPathConfigAsync();
                }

                FileSystemWatcher watcher = new()
                {
                    Filter = "*.mp3",
                    Path = libraryFilePath,
                };

                watcher.Deleted += async (sender, e) =>
                {
                    // Create a new scope for each event
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediaFileRepository = scope.ServiceProvider.GetRequiredService<IMediaFileRepository>();
                        mediaFileRepository.SetMediator(_mediator);                        
                        try
                        {
                            var fileRecord = await mediaFileRepository.GetMediaFileByFileName(e.Name);
                            await mediaFileRepository.MarkFileDeleted(fileRecord);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Exception: ", ex.Message);
                        }

                    };
                };

                watcher.Created += async (sender, e) =>
                {
                    // Create a new scope for each event
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediaFileRepository = scope.ServiceProvider.GetRequiredService<IMediaFileRepository>();
                        mediaFileRepository.SetMediator(_mediator);
                        var appConfig = scope.ServiceProvider.GetRequiredService<IAppConfigRepository>();

                        bool isFileReady = false;
                        FileInfo? fileInfo = null;

                        // Keep trying to open the file until it is ready
                        while (!isFileReady)
                        {
                            try
                            {
                                fileInfo = new FileInfo(e.FullPath);

                                // Try to open the file for exclusive access
                                using (FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                                {
                                    isFileReady = true;
                                }
                            }
                            catch (IOException)
                            {

                                await Task.Delay(100);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Failed to read file {e.FullPath} " +
                                    $"after multiple attempts.");
                                break;
                            }
                        }

                        if (isFileReady && fileInfo != null)
                        {
                            string nameTemplate = await appConfig.GetMediaFileNamingConventionAsync();
                            MediaFileMetadata metaData = _fileMetaDataService.ExtractMediaFileMetaData(fileInfo.Name, nameTemplate);
                            var recording = new MediaFile
                            {
                                Name = fileInfo.Name,
                                Time = fileInfo.LastWriteTimeUtc,
                                FileSize = fileInfo.Length,
                                Metadata = metaData
                            };

                            await mediaFileRepository.AddMediaFileAsync(recording);
                        }
                    }
                };

                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception:", ex);
            }
        }
    }
}
