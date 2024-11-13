namespace Model.Jira.Violations.RuleExtractions
{
    public class ComponentStoryRuleExtraction : BaseReporterRuleExtraction
    {
        public override string Jql => "type = Story AND component is EMPTY";
    }
}
