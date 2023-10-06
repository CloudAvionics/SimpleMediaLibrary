using SimpleMediaLibrary.Common;

namespace SimpleMediaLibrary.Common
{
    public interface IMediaFileEventMediator
    {
        event EventHandler<MediaFileChangedEventArgs> MediaFileChanged;
        void RaiseMediaFileChanged(MediaFileChangedEventArgs e);
    }
}
