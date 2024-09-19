namespace Model
{
    public interface IService
    {
        public bool IsReady { get; }

        public event TaskEventHandler Ready;
    }
}
