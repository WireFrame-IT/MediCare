using System.Text.Json;
using System.Text.Json.Serialization;

public class UtcToPolishTimeJsonConverter : JsonConverter<DateTime>
{
	private static readonly TimeZoneInfo PolishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var utc = DateTime.SpecifyKind(reader.GetDateTime(), DateTimeKind.Utc);
		return TimeZoneInfo.ConvertTimeFromUtc(utc, PolishTimeZone);
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		var unspecified = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
		var utc = TimeZoneInfo.ConvertTimeToUtc(unspecified, PolishTimeZone);
		writer.WriteStringValue(utc.ToString("o"));
	}
}