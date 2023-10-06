using Persistence.Model;
using SimpleMediaLibrary.Common;

namespace DataAccessService.DataAccess
{
    public interface IMediaFileRepository
    {
        Task AddMediaFileAsync(MediaFile file);
        Task<IEnumerable<MediaFile>> GetMediaFiles(bool includeDeleted = false);
        Task<MediaFile> GetMediaFileByFileName(string filename, bool includeDeleted = false);
        Task RemoveMediaFileAsync(MediaFile mediaFile);
        Task MarkFileDeleted(MediaFile mediaFile);

        event EventHandler<MediaFileChangedEventArgs> MediaFileChanged;
        void SetMediator(IMediaFileEventMediator mediator);


    }
}
