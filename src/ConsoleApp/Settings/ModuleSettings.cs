using ConsoleApp.Settings.Rules;

namespace ConsoleApp.Settings
{
    public abstract class ModuleSettings<Key> where Key : notnull
    {
        public Key Identifier { get; set; }

        public bool Disabled { get; set; } = false;

        public IDictionary<string, object>? Properties { get; set; }
    }
}
