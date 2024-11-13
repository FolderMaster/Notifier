namespace Model.Jira.Violations.RuleExtractions
{
    public class RoughEstimationStoryRuleExtraction : BaseReporterRuleExtraction
    {
        public override string Jql => "type = Story AND \"Rough estimation\" is EMPTY";
    }
}
