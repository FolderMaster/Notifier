using System.Text.Json;

namespace ConsoleApp.Data
{
    public class JsonDataBaseContext
    {
        public readonly string _fileName;

        private JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        public List<UserData> UserData { get; set; }

        public JsonDataBaseContext(string fileName)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
            _fileName = fileName;
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(UserData, _jsonOptions);
            File.WriteAllText(_fileName, json);
        }

        public void Load()
        {
            if (File.Exists(_fileName))
            {
                var json = File.ReadAllText(_fileName);
                var newUserData = JsonSerializer.Deserialize<List<UserData>>(json, _jsonOptions);
                UserData = newUserData != null ? newUserData : new List<UserData>();
            }
            else
            {
                UserData = new List<UserData>();
            }
        }
    }
}
