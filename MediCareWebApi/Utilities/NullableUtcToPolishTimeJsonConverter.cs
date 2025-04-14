using System.Text.Json.Serialization;
using System.Text.Json;

public class NullableUtcToPolishTimeJsonConverter : JsonConverter<DateTime?>
{
	private static readonly TimeZoneInfo PolishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

	public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
			return null;

		var utc = DateTime.SpecifyKind(reader.GetDateTime(), DateTimeKind.Utc);
		return TimeZoneInfo.ConvertTimeFromUtc(utc, PolishTimeZone);
	}

	public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
	{
		if (value.HasValue)
			writer.WriteStringValue(value.Value.ToUniversalTime());
		else
			writer.WriteNullValue();
	}
}
