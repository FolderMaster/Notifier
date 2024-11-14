namespace ConsoleApp.JiraRules.Extractions
{
    public class ClosedStoryRuleExtraction : BaseReporterRuleExtraction
    {
        public override string Jql => "type = Story AND status = Closed";
    }
}
