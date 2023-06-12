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
    public async Task<IActionResult> MakeReservationAsync([FromServices] AppDbContext context, [FromBody] CreateReservationViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reservations = context.Reservations;
        var reservation = new Reservation
        {
            Date = model.Date,
            Time = model.Time,
            People = model.People
        };

        if (reservations.Any(res => res.Date == reservation.Date && res.Time == reservation.Time))
            return Conflict("Date and Time already booked");

        try
        {
            await reservations.AddAsync(reservation);
            await context.SaveChangesAsync();
            return Created($"v1/reservations/{reservation.Id}", reservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}