using Model.Jira.Violations;

namespace ConsoleApp.Inspection
{
    public interface IInspector
    {
        public Task<IEnumerable<JiraViolation>> Inspect();
    }
}
