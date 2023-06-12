using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Data.Abstractions.Converters;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return DateOnly.ParseExact(reader.GetString()!, DateFormat);
        }
        catch
        {
            throw new JsonException($"The field Date must follow the template '{DateFormat}'");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        var date = value.ToString("0");
        writer.WriteStringValue(date);
    }
}