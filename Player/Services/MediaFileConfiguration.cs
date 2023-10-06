namespace Player.Services
{
    public class MediaFileConfiguration : IMediaFileConfiguration
    {
        public string MediaFilesPath { get; private set; }

        public void UpdateAudioFilePath(string newPath)
        {
            MediaFilesPath = newPath;
        }
    }

}
