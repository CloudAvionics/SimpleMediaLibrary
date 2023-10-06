using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using DataAccessService.DataAccess;
using Persistence.Model;

namespace SimpleMediaLibrary.FileManagementService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher();

                watcher.Filter = "*.mp3";

                watcher.Created += async (sender, e) =>
                {
                    // Create a new scope for each event
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediaFileRepository = scope.ServiceProvider.GetRequiredService<IMediaFileRepository>();
                        var appConfig = scope.ServiceProvider.GetRequiredService<IAppConfigRepository>();

                        watcher.Path = await appConfig.GetRecordingPathConfigAsync();

                        bool isFileReady = false;
                        FileInfo fileInfo = null;

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
                                _logger.LogError($"Failed to read file {e.FullPath} after multiple attempts.");
                                break;
                            }
                        }

                        if (isFileReady && fileInfo != null)
                        {
                            var recording = new MediaFile
                            {
                                Name = fileInfo.Name,
                                Time = fileInfo.LastWriteTimeUtc,
                                FileSize = fileInfo.Length
                            };

                            await mediaFileRepository.AddMediaFileAsync(recording);
                        }
                    }
                };
                // Initialize the path when the service starts.
                using (var initialScope = _serviceProvider.CreateScope())
                {
                    var appConfig = initialScope.ServiceProvider.GetRequiredService<IAppConfigRepository>();
                    watcher.Path = await appConfig.GetRecordingPathConfigAsync();
                }

                watcher.EnableRaisingEvents = true;

                //while (!stoppingToken.IsCancellationRequested)
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //    await Task.Delay(1000, stoppingToken);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception:", ex);
            }
        }
    }
}