using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RestaurantReservation.Data.Abstractions.Attributes;
using RestaurantReservation.Data.Abstractions.Converters;

namespace RestaurantReservation.Application.ViewModels;

public class MakeReservationViewModel
{
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Date { get; set; }
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    [TimeRange("18:00:00", "23:00:00")]
    public TimeOnly Time { get; set; }
    [Range(1, 10, ErrorMessage = "The field People must be greater than zero and lower than 10")]
    public int People { get; set; }
    [Required(ErrorMessage = "The field Phone is mandatory.")]
    [RegularExpression("^(\\d{13})$", ErrorMessage = "Phone must have 13 digits.")]
    public string Phone { get; set; }
}