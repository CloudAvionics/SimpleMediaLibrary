namespace Player.Services
{
    public class BlazorEventService : IBlazorEventService
    {
        public event Action OnChange;
        public void NotifyDataChanged() => OnChange?.Invoke();
    }
}
