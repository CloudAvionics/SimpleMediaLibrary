namespace Persistence.Model
{
    public class MediaFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public long FileSize { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedTime { get; set; }
        public MediaFileMetadata? Metadata { get; set; }
    }
}
