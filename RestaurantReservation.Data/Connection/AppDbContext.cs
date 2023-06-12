using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Data.Connection;

public class AppDbContext :  DbContext
{
    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseSqlite("Data Source=../RestaurantReservation.Data/restaurant.db;Cache=Shared");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>().Ignore(reservation => reservation.Table);
        base.OnModelCreating(modelBuilder);
    }
}