namespace RestaurantReservation.Domain.Models;

public class Table
{
    public int Id { get; set; }
    public int Seats { get; set; }
    public List<Reservation> Reservations { get; set; }

    public bool IsAvailable(DateOnly date, TimeOnly time)
    {
        foreach (var reservation in Reservations)
        {
            if (reservation.Date != date)
                continue;

            if (time >= reservation.Time.Add(new TimeSpan(0, -59, -59)) && time <= reservation.Time.Add(new TimeSpan(0, 59, 59)))
                return false;
        }

        return true;
    }
}