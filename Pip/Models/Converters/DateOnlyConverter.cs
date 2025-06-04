using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pip.UI.Models.Converters;

public class DateOnlyConverter : JsonConverter<DateOnly?>
{
	public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String)
			throw new ArgumentException($"Invalid JsonTokenType {reader.TokenType}, expected String");
        string? str = reader.GetString();
        return string.IsNullOrEmpty(str) ? null : DateOnly.Parse(str, CultureInfo.InvariantCulture);
	}

	public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, value, options);
	}
}
