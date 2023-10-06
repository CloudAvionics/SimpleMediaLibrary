using Persistence.Model;

namespace SimpleMediaLibrary.Common;

public class MediaFileChangedEventArgs : EventArgs
{
    public MediaFile ChangedMediaFile { get; }
    public ChangeType TypeOfChange { get; }

    public MediaFileChangedEventArgs(MediaFile changedMediaFile,
                                     ChangeType typeOfChange)
    {
        ChangedMediaFile = changedMediaFile;
        TypeOfChange = typeOfChange;
    }
}

public enum ChangeType
{
    Added,
    Deleted,
    Updated,
    MarkedDeleted
}
