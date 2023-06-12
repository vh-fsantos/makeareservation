using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Abstractions.Connection;

namespace RestaurantReservation.Controllers;

[ApiController]
[Route("v1/tables")]
public class TableController : ControllerBase
{
    private readonly IAppDbContext _appDbContext;

    public TableController([FromServices] IAppDbContext context)
    {
        _appDbContext = context;
    }

    [HttpGet]
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

        var tables = await _appDbContext.Tables.AsNoTracking().Where(table => table.Seats == model.People).Include(table => table.Reservations).ToListAsync();
        var availableTables = tables.Where(table => table.IsAvailable(model.Date, model.Time)).ToList();

        return availableTables.Count == 0
            ? NotFound("No available tables")
            : Ok($"There's {availableTables.Count} available tables at {model.Date} on {model.Time} for {model.People} people.");
    }
}