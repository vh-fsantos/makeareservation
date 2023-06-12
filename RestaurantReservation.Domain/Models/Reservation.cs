using System.Text.Json.Serialization;

namespace RestaurantReservation.Domain.Models;

public class Reservation
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int People { get; set; }
    public string Phone { get; set; }
    public int TableId { get; set; }
    [JsonIgnore]
    public Table Table { get; set; }
}