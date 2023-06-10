using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Data.Connection;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Application.Controllers;

[ApiController]
[Route("v1")]
public class ReservationController : ControllerBase
{
    [HttpGet]
    [Route("reservations")]
    public async Task<IActionResult> GetReservations([FromServices] AppDbContext context)
    {
        var reservations = await context.Reservations.AsNoTracking().ToListAsync();
        return Ok(reservations);
    }
}