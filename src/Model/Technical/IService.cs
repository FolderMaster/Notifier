namespace Model.Technical
{
    public interface IService
    {
        public bool IsReady { get; }

        public event TaskEventHandler Ready;
    }
}
