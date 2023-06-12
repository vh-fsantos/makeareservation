using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReservation.Domain.Models;

public class ReservationTable
{
    [ForeignKey("ReservationId")]
    public int ReservationId { get; set; }
    [ForeignKey("TableId")]
    public int TableId { get; set; }
}