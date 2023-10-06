using Persistence.Model;

namespace Player.Services
{
    public interface IMediaFileMetaDataService
    {
        MediaFileMetadata ExtractMediaFileMetaData(string filename, string nameTemplate);
    }
}