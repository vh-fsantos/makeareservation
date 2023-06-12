using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        var people = model.People;
        var date = model.Date;
        var time = model.Time;
        var availableTables = await context.Tables.AsNoTracking().Where(table => table.Seats == people).Include(table => table.Reservations).ToListAsync();
        var availableTable = availableTables.FirstOrDefault(table => table.IsAvailable(date, time));

        if (availableTable == null)
            return Conflict("No available tables");

        var reservation = new Reservation
        {
            Date = date,
            Time = time,
            People = people,
            TableId = availableTable.Id,
            Phone = model.Phone
        };

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

    [HttpDelete("reservations/{reservationId}")]
    public async Task<IActionResult> CancelReservationAsync([FromServices] AppDbContext context, [FromRoute] int reservationId)
    {
        var reservation = await context.Reservations.FirstOrDefaultAsync(res => res.Id == reservationId);

        if (reservation == null)
            return BadRequest("Reservation doesn't exists");

        try
        {
            context.Reservations.Remove(reservation);
            await context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}