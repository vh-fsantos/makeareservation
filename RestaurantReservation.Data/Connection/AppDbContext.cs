using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Data.Connection;

public class AppDbContext :  DbContext
{
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseSqlite("Data Source=../RestaurantReservation.Data/restaurant.db;Cache=Shared", options => options.MigrationsAssembly("RestaurantReservation.Data"));
}