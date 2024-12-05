namespace ConsoleApp.Inspection
{
    public record DbInspectorViolation
    {
        public string IssueId { get; set; }

        public string UserId { get; set; }

        public Type InspectorType { get; set; }
    }
}
