namespace TrainsOnline.Desktop.Application.Extensions
{
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class JsonExtensions
    {
        public static async Task<T> ToObjectAsync<T>(this string value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings);
            });
        }

        public static async Task<string> ToJsonAsync(this object value)
        {
            string json = await Task.Run(() =>
            {
                return JsonConvert.SerializeObject(value);
            });

            return await json.JsonPrettifyAsync();
        }

        public static T ToObject<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static string ToPrettyJson(this object value)
        {
            string json = JsonConvert.SerializeObject(value);

            return json.JsonPrettify();
        }

        public static string JsonPrettify(this string json)
        {
            using (StringReader stringReader = new StringReader(json))
            using (StringWriter stringWriter = new StringWriter())
            {
                JsonTextReader jsonReader = new JsonTextReader(stringReader);
                JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter)
                {
                    Formatting = Formatting.Indented
                };
                jsonWriter.WriteToken(jsonReader);

                return stringWriter.ToString();
            }
        }

        public static async Task<string> JsonPrettifyAsync(this string json)
        {
            using (StringReader stringReader = new StringReader(json))
            using (StringWriter stringWriter = new StringWriter())
            {
                JsonTextReader jsonReader = new JsonTextReader(stringReader);
                JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter)
                {
                    Formatting = Formatting.Indented
                };
                await jsonWriter.WriteTokenAsync(jsonReader);

                return stringWriter.ToString();
            }
        }
    }
}
