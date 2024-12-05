namespace ConsoleApp.Inspection
{
    public interface IExecutor
    {
        public Task Execute(IEnumerable<InspectorViolation> violations);
    }
}
