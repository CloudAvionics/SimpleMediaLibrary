namespace SimpleMediaLibrary.Common;

public class MediaFileEventMediator : IMediaFileEventMediator
{
    public event EventHandler<MediaFileChangedEventArgs> MediaFileChanged;

    public void RaiseMediaFileChanged(MediaFileChangedEventArgs e)
    {
        MediaFileChanged?.Invoke(this, e);
    }
}