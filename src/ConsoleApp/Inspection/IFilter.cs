namespace ConsoleApp.Inspection
{
    public interface IFilter
    {
        public IEnumerable<InspectorViolation> Filter(IEnumerable<InspectorViolation> violations);
    }
}
