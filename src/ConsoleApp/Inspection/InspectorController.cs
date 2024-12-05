namespace ConsoleApp.Inspection
{
    public class InspectorController
    {
        private IEnumerable<IInspector> _inspectors;

        private IFilter _filter;

        private IEnumerable<IExecutor> _executors;

        public InspectorController(IEnumerable<IInspector> inspectors,
            IEnumerable<IExecutor> executors, IFilter filter)
        {
            _inspectors = inspectors;
            _filter = filter;
            _executors = executors;
        }

        public async Task FindViolations()
        {
            var inspectorViolations = new List<InspectorViolation>();
            foreach (var inspector in _inspectors)
            {
                var violations = await inspector.Inspect();
                foreach (var violation in violations)
                {
                    inspectorViolations.Add(new InspectorViolation(violation, inspector));
                }
            }
            var filteredInspectorViolations = _filter.Filter(inspectorViolations);
            foreach (var executor in _executors)
            {
                await executor.Execute(filteredInspectorViolations);
            }
        }
    }
}
