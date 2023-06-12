using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RestaurantReservation.Application.Services;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Connection;
using RestaurantReservation.Domain.Models;
using RestaurantReservation.Application.Tests.Helpers;

namespace RestaurantReservation.Application.Tests.Services;

[TestFixture]
public class RestaurantApplicationServiceTests
{
    private Mock<AppDbContext> _appDbContextMock;
    private Mock<DbSet<Reservation>> _dbSetReservationMock;

    [SetUp]
    public void SetUp()
    {
        _appDbContextMock = new Mock<AppDbContext>();
        SetupReservationMocks();
        _appDbContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Test]
    public async Task GetPastReservationsAsync_ReturnsReservations_WhenValidRequest()
    {
        // Arrange
        var service = new RestaurantApplicationService(_appDbContextMock.Object);
        var model = new GetPastReservationsViewModel
        {
            Phone = "5554999267175"
        };

        // Act
        var result = await service.GetPastReservationsAsync(model);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetPastReservationsAsync_ReturnsEmptyList_WhenPhoneDontExists()
    {
        // Arrange
        var service = new RestaurantApplicationService(_appDbContextMock.Object);
        var model = new GetPastReservationsViewModel
        {
            Phone = "5554999267174"
        };

        // Act
        var result = await service.GetPastReservationsAsync(model);

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }

    private void SetupReservationMocks()
    {
        _dbSetReservationMock = new Mock<DbSet<Reservation>>();
        var reservations = CreateReservationsList().AsQueryable();
        _dbSetReservationMock.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(reservations.Provider);
        _dbSetReservationMock.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(reservations.Expression);
        _dbSetReservationMock.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(reservations.ElementType);
        _dbSetReservationMock.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(() => reservations.GetEnumerator());
        _dbSetReservationMock.As<IAsyncEnumerable<Reservation>>().Setup(dbSet => dbSet.GetAsyncEnumerator(default)).Returns(new AsyncEnumerator<Reservation>(reservations.GetEnumerator()));
        _appDbContextMock.Setup(context => context.Reservations).Returns(_dbSetReservationMock.Object);
    }

    private IEnumerable<Reservation> CreateReservationsList()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var now = TimeOnly.FromDateTime(DateTime.Now);
        return new List<Reservation>
        {
            new() { Id = 1, Phone = "5554999267176", Date = new DateOnly(today.Year, today.Month, today.Day + 1), Time = now },
            new() { Id = 2, Phone = "5554999267175", Date = new DateOnly(today.Year, today.Month, today.Day - 1), Time = now },
            new() { Id = 3, Phone = "5554999267175", Date = today, Time = new TimeOnly(now.Hour, now.Minute - 1, now.Second) }
        };
    }
}