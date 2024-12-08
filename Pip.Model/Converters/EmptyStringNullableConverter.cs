using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pip.Model.Converters;

public class EmptyStringNullableConverter<T> : JsonConverter<T?> where T : struct
{
	public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String) return JsonSerializer.Deserialize<T>(ref reader, options);
		var value = reader.GetString();
		return string.IsNullOrEmpty(value) ? null : JsonSerializer.Deserialize<T>(ref reader, options);
	}

	public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, value, options);
	}
}