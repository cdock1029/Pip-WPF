using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using DateOnly = System.DateOnly;

namespace Pip.Model;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private const string Format = "s";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new ArgumentException($"Invalid JsonTokenType {reader.TokenType}, expected String");
        var str = reader.GetString();
        return string.IsNullOrEmpty(str) ? default : DateOnly.ParseExact(str, Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
