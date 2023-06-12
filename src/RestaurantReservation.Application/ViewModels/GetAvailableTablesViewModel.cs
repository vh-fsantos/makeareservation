using System.ComponentModel.DataAnnotations;
using RestaurantReservation.Data.Abstractions.Attributes;

namespace RestaurantReservation.Application.ViewModels;

public class GetAvailableTablesViewModel
{
    public DateOnly Date { get; set; }
    [TimeRange("18:00:00", "23:00:00")]
    public TimeOnly Time { get; set; }
    [Range(1, 10, ErrorMessage = "The field People must be greater than 0 and lower than 10")]
    public int People { get; set; }
}