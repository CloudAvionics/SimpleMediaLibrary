using DataAccessService.DataAccess;
using Persistence.Model;
using SimpleMediaLibrary.Common;

namespace Player.Services
{
    public class FileSyncService : IFileSyncService
    {
        private readonly ILogger<FileSyncService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediaFileEventMediator _mediator;
        private readonly IMediaFileConfiguration _mediaFileConfiguration;
        private readonly IMediaFileMetaDataService _mediaFileMetaDataService;
        string libraryFilePath = String.Empty;

        public FileSyncService(ILogger<FileSyncService> logger,
            IServiceProvider serviceProvider,
            IMediaFileEventMediator mediator,
            IMediaFileConfiguration mediaFileConfiguration,
            IMediaFileMetaDataService mediaFileMetaDataService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _mediaFileConfiguration = mediaFileConfiguration;
            _mediaFileMetaDataService = mediaFileMetaDataService;
        }

        public async Task TaskSyncFilesWithDatabaseAsync()
        {
            using (var initialScope = _serviceProvider.CreateScope())
            {
                var appConfig = initialScope.ServiceProvider.GetRequiredService<IAppConfigRepository>();
                libraryFilePath = await appConfig.GetRecordingPathConfigAsync();

                var mediaFileRepository = initialScope.ServiceProvider.GetRequiredService<IMediaFileRepository>();
                mediaFileRepository.SetMediator(_mediator);

                await SyncFilesWithDatabase(libraryFilePath, mediaFileRepository, appConfig);

                // Set the MediaFileConfig
                _mediaFileConfiguration.UpdateAudioFilePath(libraryFilePath);
            }
        }
        private async Task SyncFilesWithDatabase(string directoryPath,
                                         IMediaFileRepository mediaFileRepository,
                                         IAppConfigRepository appConfig)
        {
            try
            {
                // Get all the MediaFiles from database
                var mediaFilesFromDb = await mediaFileRepository.GetMediaFiles();
                var mediaFileNamesFromDb = mediaFilesFromDb
                    .Select(m => m.Name).ToList(); // Convert to List of Names


                // Get all the actual files from the disk
                var actualFiles = Directory.GetFiles(directoryPath, "*.mp3")
                    .Select(f => Path.GetFileName(f));

                // Identify the files that are in database but not on disk
                var missingFiles = mediaFilesFromDb
                    .Where(dbFile => !actualFiles
                    .Contains(dbFile.Name))
                    .ToList();

                // Identify the files that are on disk but not in the database
                var extraFiles = actualFiles
                    .Where(diskFile => !mediaFileNamesFromDb.Contains(diskFile))
                    .ToList();

                // Mark these entries as deleted in the database
                foreach (var missingFile in missingFiles)
                {
                    await mediaFileRepository.MarkFileDeleted(missingFile);
                }

                // Add the extra files to the database
                foreach (var extraFileName in extraFiles)
                {
                    var fullPath = Path.Combine(directoryPath, extraFileName);
                    var extraFile = new FileInfo(fullPath);

                    if (extraFile.Exists) // Check if the file exists to be sure
                    {
                        string nameTemplate = await appConfig
                            .GetMediaFileNamingConventionAsync();
                        
                        MediaFileMetadata meta = _mediaFileMetaDataService
                            .ExtractMediaFileMetaData(extraFileName, nameTemplate);

                        var fileItem = new MediaFile
                        {
                            Name = extraFile.Name,
                            Time = extraFile.LastWriteTimeUtc,
                            FileSize = extraFile.Length,
                            Metadata = meta
                        };

                        await mediaFileRepository.AddMediaFileAsync(fileItem);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SyncFilesWithDatabase Exception: ", ex.Message);
            }
        }
    }
}
