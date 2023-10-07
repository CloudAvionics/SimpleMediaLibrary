using Persistence.Model;
using System.Globalization;

namespace Player.Services
{
    public class MediaFileMetaDataService : IMediaFileMetaDataService
    {
        private readonly ILogger<MediaFileMetaDataService> _logger;

        public MediaFileMetaDataService(ILogger<MediaFileMetaDataService> logger)
        {
            _logger = logger;
        }

        public MediaFileMetadata? ExtractMediaFileMetaData(string fileName, string nameTemplate)
        {
            try
            {
                // Remove file extension
                fileName = fileName.Substring(0, fileName.LastIndexOf('.'));

                // Split template and filename into parts
                var templateParts = nameTemplate.Split(new[] { '_' }, StringSplitOptions.None);
                var fileNameParts = fileName.Split(new[] { '_' }, StringSplitOptions.None);

                if (templateParts.Length != fileNameParts.Length)
                {
                    _logger.LogError("Invalid file name based on template.");
                    throw new InvalidDataException("Invalid file name based on template");
                }

                var keysDict = new Dictionary<string, string>();

                for (int i = 0; i < templateParts.Length; i++)
                {
                    var key = templateParts[i].Trim('{', '}');
                    var value = fileNameParts[i];
                    keysDict[key] = value;
                }

                MediaFileMetadata metadata = new MediaFileMetadata();
                var actionMap = new Dictionary<string, Action<string>>
            {
                { "author", value => metadata.Author = value },
                { "content", value => metadata.Content = value },
                { "description", value => metadata.Description = value },
                { "frequency", value => metadata.Frequency = value },
                { "genre", value => metadata.Genre = value },
                { "notes", value => metadata.Notes = value },
                { "publishdate", value =>
                    {
                        if (DateTime.TryParseExact(value,
                                "yyyyMMdd",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime date))
                        {
                            metadata.PublishDate = date;
                        }
                        else
                        {

                        }
                    }
                },
                { "publishtime", value =>
                    {
                        if (int.TryParse(value, out int hour))
                        {
                            metadata.PublishTime = new DateTime(1, 1, 1, hour, 0, 0);
                        }
                    }
                },
                { "station", value => metadata.Station = value },
                { "title", value => metadata.Title = value }
            };

                // Loop over the metadata and apply actions based on keys
                foreach (var metaEntry in keysDict)
                {
                    if (actionMap.TryGetValue(metaEntry.Key, out var action))
                    {
                        action(metaEntry.Value);
                    }
                }

                return metadata;

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception: {ex.Message}");
                return null;
            }
        }
    }
}

