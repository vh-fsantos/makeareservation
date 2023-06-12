namespace RestaurantReservation.Domain.Models;

public class Table
{
    public int Id { get; set; }
    public int Seats { get; set; }
    public List<Reservation> Reservations { get; set; }

    public bool IsAvailable(DateOnly date, TimeOnly time)
    {
        if (time < new TimeOnly(18, 0, 0) || time > new TimeOnly(23, 0, 0) || date < DateOnly.FromDateTime(DateTime.Now))
            return false;

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