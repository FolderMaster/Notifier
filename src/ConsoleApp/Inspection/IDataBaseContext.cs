namespace ConsoleApp.Inspection
{
    public interface IDataBaseContext<T>
    {
        public List<T> Data { get; }

        public void Save();

        public void Load();
    }
}
