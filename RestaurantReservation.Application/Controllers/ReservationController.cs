using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Connection;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Application.Controllers;

[ApiController]
[Route("v1")]
public class ReservationController : ControllerBase
{
    [HttpGet("reservations")]
    public async Task<IActionResult> GetReservationsAsync([FromServices] AppDbContext context)
    {
        var reservations = await context.Reservations.AsNoTracking().ToListAsync();
        return Ok(reservations);
    }

    [HttpPost("reservations")]
    public async Task<IActionResult> MakeReservationAsync([FromServices] AppDbContext context, [FromBody] MakeReservationViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var availableTables = await context.Tables.ToListAsync();
        var reservation = new Reservation
        {
            Date = model.Date,
            Time = model.Time,
            People = model.People,
        };
        var reservations = await context.Reservations.AsNoTracking().Where(res => res.Date == reservation.Date).ToListAsync();

        foreach (var table in availableTables.Where(tb => tb.Seats == reservation.People))
        {
            var currentReservation = reservations.FirstOrDefault(res => (reservation.Time >= res.Time.Add(new TimeSpan(0, -59, -59)) || 
                                                                 reservation.Time <= res.Time.Add(new TimeSpan(0, 59, 59))) && res.TableId == table.Id);

            if (currentReservation == null)
                continue;

            reservation.TableId = table.Id;
            break;
        }

        if (reservation.TableId == -1)
            return Conflict("No available tables");

        try
        {
            await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();
            return Created($"v1/reservations/{reservation.Id}", reservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}