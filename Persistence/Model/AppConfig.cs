namespace Persistence.Model
{
    public class AppConfig
    {
        public int Id { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; } = string.Empty;
    }
}
