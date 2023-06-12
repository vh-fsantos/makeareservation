using System.ComponentModel.DataAnnotations;
using RestaurantReservation.Data.Attributes;

namespace RestaurantReservation.Application.ViewModels;

public class CreateReservationViewModel
{
    [Required(ErrorMessage = "The field Date is mandatory.")]
    [DataType(DataType.Date, ErrorMessage = "The field Date must follow this template 'yyyy-mm-dd'")]
    [DateRange("2023-06-05", "2023-12-05")]
    public DateOnly Date { get; set; }
    [Required(ErrorMessage = "The field Time is mandatory.")]
    [TimeRange("08:00:00", "18:00:00")]
    public TimeOnly Time { get; set; }
    [Required(ErrorMessage = "The field People is mandatory.")]
    [Range(1, 10, ErrorMessage = "The field People must be greater than zero and lower than 10")]
    public int People { get; set; }
}