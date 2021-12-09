using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Deveel.Math;

namespace CelestialObjectCatalog.WebApi.Converters
{
    public class BigDecimalConverter : JsonConverter<BigDecimal>
    {
        public override BigDecimal Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) 
                => BigDecimal.Parse(reader.GetString() ?? string.Empty);

        public override void Write(
            Utf8JsonWriter writer,
            BigDecimal value,
            JsonSerializerOptions options)
                => writer
                    .WriteStringValue(value.ToString());
    }
}
