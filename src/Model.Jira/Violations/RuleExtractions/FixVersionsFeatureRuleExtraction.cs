namespace Model.Jira.Violations.RuleExtractions
{
    public class FixVersionsFeatureRuleExtraction : BaseReporterRuleExtraction
    {
        public override string Jql => "type = Feature AND fixVersion is EMPTY";
    }
}
