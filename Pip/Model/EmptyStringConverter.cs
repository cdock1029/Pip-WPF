using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pip.Model;

public class EmptyStringConverter<T> : JsonConverter<T?>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String) return JsonSerializer.Deserialize<T>(ref reader, options);
        var value = reader.GetString();
        // only works if [JsonConverter(typeof(EmptyStringConverter<string>))] is applied to the property, else it will throw stack overflow exception (recursion)
        return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}