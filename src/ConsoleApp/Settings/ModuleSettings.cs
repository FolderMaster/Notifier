namespace ConsoleApp.Settings
{
    public class ModuleSettings
    {
        public string Name { get; set; }

        public bool? Enabled { get; set; }

        public IDictionary<string, object>? Properties { get; set; }
    }
}
