using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RestaurantReservation.Data.Connection;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Application.Controllers;

[ApiController]
[Route("v1")]
public class TableController : ControllerBase
{
    [HttpGet("tables/date={date}&time={time}&people={people}")]
    public async Task<IActionResult> GetAvailableTablesAsync([FromServices] AppDbContext context, [FromRoute] string date, [FromRoute] string time, [FromRoute] int people)
    {
        var dateObject = DateOnly.Parse(date);
        var timeObject = TimeOnly.Parse(time);
        var tables = await context.Tables.AsNoTracking().Where(table => table.Seats == people).Include(table => table.Reservations).ToListAsync();
        var availableTables = tables.Where(table => table.IsAvailable(dateObject, timeObject)).ToList();

        return availableTables.Count == 0
            ? NotFound("No available tables")
            : Ok($"There's {availableTables.Count} available tables at {date} on {time} for {people} people.");
    }
}