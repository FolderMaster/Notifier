using ConsoleApp.Inspection;

namespace ConsoleApp.Data
{
    public class JsonDataBaseContext<T> : IDataBaseContext<T>
    {
        public readonly string _fileName;

        public List<T> Data { get; private set; }

        public JsonDataBaseContext(string fileName)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
            _fileName = fileName;
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(Data);
            File.WriteAllBytes(_fileName, json);
        }

        public void Load()
        {
            if (File.Exists(_fileName))
            {
                var json = File.ReadAllBytes(_fileName);
                var newUserData = JsonSerializer.Deserialize<List<T>>(json);
                Data = newUserData != null ? newUserData : new List<T>();
            }
            else
            {
                Data = new List<T>();
            }
        }
    }
}
