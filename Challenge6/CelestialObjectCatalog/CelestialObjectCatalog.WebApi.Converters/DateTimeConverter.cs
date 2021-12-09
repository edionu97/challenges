using System;
using System.Text.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace CelestialObjectCatalog.WebApi.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
                => DateTime.Parse(
                    reader.GetString() ?? string.Empty);

        public override void Write(
            Utf8JsonWriter writer, 
            DateTime value, 
            JsonSerializerOptions options) 
                => writer
                    .WriteStringValue(
                        value.ToString("yyyy-MM-dd"));
    }
}
