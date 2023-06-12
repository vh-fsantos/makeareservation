using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Connection;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Application.Services;

public class RestaurantApplicationService
{
    private readonly AppDbContext _context;

    public RestaurantApplicationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetPastReservationsAsync(GetPastReservationsViewModel model)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var now = TimeOnly.FromDateTime(DateTime.Now);
        var reservations = await _context.Reservations.AsNoTracking().ToListAsync();
        var query = from r in reservations
                                        where r.Phone == model.Phone && (r.Date < today || r.Date == today && r.Time < now) 
                                        select r;

        return query.ToList();
    }

    public async Task<List<Table>> GetAvailableTablesAsync(GetAvailableTablesViewModel model)
    {
        var tables = await _context.Tables.AsNoTracking().Include(table => table.Reservations).Where(table => table.Seats == model.People).ToListAsync();
        var availableTables = tables.Where(table => table.IsAvailable(model.Date, model.Time)).ToList();
        return availableTables;
    }

    public async Task<Reservation> MakeReservationAsync(MakeReservationViewModel model)
    {
        var date = model.Date;
        var time = model.Time;
        var people = model.People;
        var availableTablesModel = new GetAvailableTablesViewModel
        {
            Date = date,
            Time = time,
            People = people
        };

        var availableTable = (await GetAvailableTablesAsync(availableTablesModel)).FirstOrDefault(table => table.IsAvailable(date, time));
        if (availableTable == null)
            return null;

        var reservation = new Reservation
        {
            Date = date,
            Time = time,
            People = people,
            TableId = availableTable.Id,
            Phone = model.Phone
        };

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<bool> CancelReservationAsync(int id)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(res => res.Id == id);

        if (reservation == null)
            return false;

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }
}