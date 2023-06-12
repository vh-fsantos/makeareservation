using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Connection;

namespace RestaurantReservation.Application.Controllers;

[ApiController]
[Route("v1/tables")]
public class TableController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAvailableTablesAsync([FromServices] AppDbContext context, [FromQuery] GetAvailableTablesViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tables = await context.Tables.AsNoTracking().Where(table => table.Seats == model.People).Include(table => table.Reservations).ToListAsync();
        var availableTables = tables.Where(table => table.IsAvailable(model.Date, model.Time)).ToList();

        return availableTables.Count == 0
            ? NotFound("No available tables")
            : Ok($"There's {availableTables.Count} available tables at {model.Date} on {model.Time} for {model.People} people.");
    }
}