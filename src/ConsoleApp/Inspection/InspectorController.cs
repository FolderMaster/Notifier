using Model.Jira.Violations;

namespace ConsoleApp.Inspection
{
    public class InspectorController
    {
        private IEnumerable<IInspector> _inspectors;

        public InspectorController(IEnumerable<IInspector> inspectors)
        {
            _inspectors = inspectors;
        }

        public async Task FindViolations()
        {
            foreach (IInspector inspector in _inspectors)
            {
                IEnumerable<JiraViolation> violations = await inspector.Inspect();

                // Здесь решаем что делаем с полученными нарушениями
            }
        }
    }
}
