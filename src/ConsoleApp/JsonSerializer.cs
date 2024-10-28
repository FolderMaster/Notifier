using Newtonsoft.Json;
using System.Text;

namespace ConsoleApp
{
    public static class JsonSerializer
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings =
            new JsonSerializerSettings()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };

        public static T? Deserialize<T>(byte[] data)
        {
            var text = Encoding.Default.GetString(data);
            return JsonConvert.DeserializeObject<T>(text, _jsonSerializerSettings);
        }

        public static byte[] Serialize(object value)
        {
            var text = JsonConvert.SerializeObject(value, _jsonSerializerSettings);
            return Encoding.Default.GetBytes(text);
        }
    }
}
