using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestaurantReservation.Domain.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int People { get; set; }
    [ForeignKey("TableId")]
    public int TableId { get; set; }
    [JsonIgnore]
    public Table Table { get; set; }
}