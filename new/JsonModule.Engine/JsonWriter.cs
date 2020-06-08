using System.IO;
using Newtonsoft.Json;

namespace JsonModule.Engine
{
    public class JsonWriter
    {
        public static string NewtonsoftName { get; } = typeof(JsonConvert).Assembly.FullName;

        public void WriteToStream(Stream stream, object value)
        {
            using (var textWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(textWriter){ Formatting = Formatting.Indented })
            {
                new JsonSerializer().Serialize(jsonWriter, value);
            }
        }
    }
}
