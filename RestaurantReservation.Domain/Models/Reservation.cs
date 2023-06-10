namespace RestaurantReservation.Domain.Models;

public class Reservation
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int People { get; set; }
}