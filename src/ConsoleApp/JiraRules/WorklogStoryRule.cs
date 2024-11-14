using Model.Jira.Violations;

using ConsoleApp.JiraRules.Executors;
using ConsoleApp.JiraRules.Extractions;

namespace ConsoleApp.JiraRules
{
    public class WorklogStoryRule : JiraRule
    {
        public WorklogStoryRule(WorklogStoryRuleExtraction extraction,
            SenderRuleExecutor executor) : base(extraction, executor) { }
    }
}
