namespace Player.Services
{
    public interface IFileSyncService
    {
        Task TaskSyncFilesWithDatabaseAsync();
    }
}