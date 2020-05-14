namespace TrainsOnline.Api.Converterts
{
    using System;
    using System.Diagnostics;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
    {
        [DebuggerHidden]
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return TimeSpan.Parse(reader.GetString());
            }
            catch (Exception)
            {
                throw new JsonException();
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
