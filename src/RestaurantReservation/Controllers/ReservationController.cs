using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Abstractions.Connection;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Controllers;

[ApiController]
[Route("v1/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IAppDbContext _appDbContext;
    private readonly IRestaurantApplicationService _restaurantService;

    public ReservationController(IRestaurantApplicationService restaurantService, [FromServices] IAppDbContext context)
    {
        _appDbContext = context;
        _restaurantService = restaurantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPastReservationsAsync([FromQuery] GetPastReservationsViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var today = DateOnly.FromDateTime(DateTime.Now);
        var reservations = await _appDbContext.Reservations.AsNoTracking().Where(reservation => reservation.Phone == model.Phone && reservation.Date < today).ToListAsync();
        return reservations.Count == 0 ? NotFound("Past reservations not found.") : Ok(reservations);
    }

    [HttpPost]
    public async Task<IActionResult> MakeReservationAsync([FromBody] MakeReservationViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var date = model.Date;
        var time = model.Time;
        var today = DateOnly.FromDateTime(DateTime.Now);
        var now = TimeOnly.FromDateTime(DateTime.Now);

        if (date < today || date == today && now > time)
            return BadRequest("It's not possible to make a reservation in the past.");

        var people = model.People;
        var availableTables = await _appDbContext.Tables.AsNoTracking().Where(table => table.Seats == people).Include(table => table.Reservations).ToListAsync();
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
            await _appDbContext.Reservations.AddAsync(reservation);
            await _appDbContext.SaveChangesAsync();
            return Created($"v1/reservations/{reservation.Id}", reservation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{reservationId}")]
    public async Task<IActionResult> CancelReservationAsync([FromRoute] int reservationId)
    {
        var reservation = await _appDbContext.Reservations.FirstOrDefaultAsync(res => res.Id == reservationId);

        if (reservation == null)
            return BadRequest("Reservation doesn't exists");

        try
        {
            _appDbContext.Reservations.Remove(reservation);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}