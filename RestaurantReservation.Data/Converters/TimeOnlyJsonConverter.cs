using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Data.Converters;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "HH:mm:ss";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return TimeOnly.ParseExact(reader.GetString()!, TimeFormat);
        }
        catch
        {
            throw new JsonException($"The field Time must follow the template '{TimeFormat}'");
        }
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        var date = value.ToString("0");
        writer.WriteStringValue(date);
    }
}