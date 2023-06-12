using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Domain.Models;

public class Table
{
    [Key]
    public int Id { get; set; }
    public int Seats { get; set; }
}