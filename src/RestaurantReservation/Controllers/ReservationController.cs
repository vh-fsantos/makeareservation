using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.Services;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Connection;

namespace RestaurantReservation.Controllers;

[ApiController]
[Route("v1/reservations")]
public class ReservationController : ControllerBase
{
    private readonly RestaurantApplicationService _restaurantService;

    public ReservationController([FromServices] AppDbContext context)
    {
        _restaurantService = new RestaurantApplicationService(context);
    }

    [HttpGet]
    public async Task<IActionResult> GetPastReservationsAsync([FromQuery] GetPastReservationsViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reservations = await _restaurantService.GetPastReservationsAsync(model);
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

        try
        {
            var reservation = await _restaurantService.MakeReservationAsync(model);

            if (reservation == null)
                return Conflict("No available tables");

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
        try
        {
            if (await _restaurantService.CancelReservationAsync(reservationId))
                return Ok();

            return BadRequest("Reservation doesn't exists");
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("availability")]
    public async Task<IActionResult> GetAvailableTablesAsync([FromQuery] GetAvailableTablesViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var date = model.Date;
        var time = model.Time;
        var today = DateOnly.FromDateTime(DateTime.Now);
        var now = TimeOnly.FromDateTime(DateTime.Now);

        if (date < today || date == today && now > time)
            return BadRequest("It's not possible to get available tables for the past.");

        var tables = await _restaurantService.GetAvailableTablesAsync(model);
        return tables.Count == 0
            ? NotFound("No available tables")
            : Ok($"There's {tables.Count} available tables at {model.Date} on {model.Time} for {model.People} people.");
    }
}