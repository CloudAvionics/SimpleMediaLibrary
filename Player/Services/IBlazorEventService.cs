namespace Player.Services
{
    public interface IBlazorEventService
    {
        event Action OnChange;

        void NotifyDataChanged();
    }
}