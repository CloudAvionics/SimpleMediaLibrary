namespace Player.Services
{
    public interface IMediaFileConfiguration
    {
        string MediaFilesPath { get; }

        void UpdateAudioFilePath(string newPath);
    }
}