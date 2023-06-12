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
        var reservations = await context.Reservations.AsNoTracking().ToListAsync();
        var tables = await context.Tables.AsNoTracking().ToListAsync();


        var availableTables = (from reservation in reservations
                               join table in tables
                                on reservation.TableId equals table.Id
                                where table.Seats == people && !(reservation.Date == dateObject && reservation.Time.Add(new TimeSpan(0, -59, -59)) <= timeObject && reservation.Time.Add(new TimeSpan(0, 59, 59)) >= timeObject)
                                select table).Count();

        return availableTables == 0
            ? NotFound("No available tables")
            : Ok($"There's {availableTables} available tables at {date} on {time} for {people} people.");
    }
}