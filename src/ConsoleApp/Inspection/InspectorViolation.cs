using Model.Jira.Violations;

namespace ConsoleApp.Inspection
{
    public record InspectorViolation : JiraViolation
    {
        public IInspector Inspector { get; private set; }

        public InspectorViolation(JiraViolation violation, IInspector inspector) :
            base(violation.User, violation.Issue)
        {
            Inspector = inspector;
        }
    }
}
