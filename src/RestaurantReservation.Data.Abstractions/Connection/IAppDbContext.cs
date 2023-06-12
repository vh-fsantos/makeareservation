using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Data.Abstractions.Connection;

public interface IAppDbContext
{
    DbSet<Table> Tables { get; set; }
    DbSet<Reservation> Reservations { get; set; }

    Task<int> SaveChangesAsync();
}