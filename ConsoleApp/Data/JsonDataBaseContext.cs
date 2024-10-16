namespace ConsoleApp.Data
{
    public class JsonDataBaseContext
    {
        public readonly string _fileName;

        public List<UserData> UserData { get; set; }

        public JsonDataBaseContext(string fileName)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
            _fileName = fileName;
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(UserData);
            File.WriteAllBytes(_fileName, json);
        }

        public void Load()
        {
            if (File.Exists(_fileName))
            {
                var json = File.ReadAllBytes(_fileName);
                var newUserData = JsonSerializer.Deserialize<List<UserData>>(json);
                UserData = newUserData != null ? newUserData : new List<UserData>();
            }
            else
            {
                UserData = new List<UserData>();
            }
        }
    }
}
