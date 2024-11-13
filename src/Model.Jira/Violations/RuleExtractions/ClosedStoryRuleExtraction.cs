namespace Model.Jira.Violations.RuleExtractions
{
    public class ClosedStoryRuleExtraction : BaseReporterRuleExtraction
    {
        public override string Jql => "type = Story AND status = Closed";
    }
}
