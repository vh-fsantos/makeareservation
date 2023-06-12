using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.ViewModels;

public class GetPastReservationsViewModel
{
    [Required(ErrorMessage = "The field Phone is mandatory.")]
    [RegularExpression("^(\\d{13})$", ErrorMessage = "Phone must have 13 digits.")]
    public string Phone { get; set; }
}