using ConsoleApp.Settings.Rules;

namespace ConsoleApp.Settings.InspectorSettings
{
    public class StoryWorklogingSettings
    {
        public RuleIdentifier Identifier { get; set; }
        public bool Enabled { get; set; }
        public DateTime? InspectionStartDate { get; set; }
        public string ViolationMessage { get; set; } = string.Empty;
    }
}
