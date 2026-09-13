using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cpa.Presentation.API.Converters;

public class JsonDateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        //var stringdate = reader.GetString()!;
        //var dateOnly = DateOnly.Parse(reader.GetString()!);
        //return DateOnly.Parse(reader.GetString()!);

        var value = reader.GetString();
        return DateOnly.ParseExact(value!,"yyyy-MM-dd",CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {       
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}
