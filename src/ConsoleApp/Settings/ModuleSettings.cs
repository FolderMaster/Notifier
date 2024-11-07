namespace ConsoleApp.Settings
{
    public class ModuleSettings
    {
        public bool Disabled { get; set; } = false;

        public IDictionary<string, object>? Properties { get; set; }
    }
}
