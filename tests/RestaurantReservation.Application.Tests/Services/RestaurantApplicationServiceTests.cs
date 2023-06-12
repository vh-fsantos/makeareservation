using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantReservation.Application.Services;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Connection;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Application.Tests.Services;

[TestFixture]
public class RestaurantApplicationServiceTests
{
    private Mock<AppDbContext> _appDbContextMock;

    [SetUp]
    public void SetUp()
    {
        _appDbContextMock = new Mock<AppDbContext>();
        _appDbContextMock.Setup(context => context.Reservations).ReturnsDbSet(CreateReservationsList());
        _appDbContextMock.Setup(context => context.Tables).ReturnsDbSet(CreateTableList());
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
        Assert.That(result.Count, Is.EqualTo(1));
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

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task CancelReservation_ReturnsTrue_WhenIdExists(int id)
    {
        // Arrange
        var service = new RestaurantApplicationService(_appDbContextMock.Object);
        
        // Act
        var result = await service.CancelReservationAsync(id);

        // Assert
        Assert.That(result, Is.True);
        _appDbContextMock.Verify(context => context.SaveChangesAsync(default), Times.Once);
        _appDbContextMock.Verify(context => context.Reservations.Remove(It.IsAny<Reservation>()), Times.Once);
    }

    [TestCase(-1)]
    [TestCase(4)]
    [TestCase(5)]
    public async Task CancelReservation_ReturnsFalse_WhenIdNotExists(int id)
    {
        // Arrange
        var service = new RestaurantApplicationService(_appDbContextMock.Object);

        // Act
        var result = await service.CancelReservationAsync(id);

        // Assert
        Assert.That(result, Is.False);
        _appDbContextMock.Verify(context => context.Reservations.Remove(It.IsAny<Reservation>()), Times.Never);
        _appDbContextMock.Verify(context => context.SaveChangesAsync(default), Times.Never);
    }

    [Test]
    public async Task GetAvailableTablesAsync_ReturnsTables_WhenValidRequest()
    {
        // Arrange
        var service = new RestaurantApplicationService(_appDbContextMock.Object);
        var model = new GetAvailableTablesViewModel
        {
            Date = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
            Time = new TimeOnly(20, 0, 0),
            People = 5
        };

        // Act
        var result = await service.GetAvailableTablesAsync(model);

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetAvailableTablesAsync_ReturnsEmpty_WhenInvalidRequest()
    {
        // Arrange
        var service = new RestaurantApplicationService(_appDbContextMock.Object);
        var model = new GetAvailableTablesViewModel
        {
            Date = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
            People = 5
        };

        // Act
        var result = await service.GetAvailableTablesAsync(model);

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task MakeReservationAsync_ReturnsReservation_WhenValidRequest()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);
        var service = new RestaurantApplicationService(_appDbContextMock.Object);
        var model = new MakeReservationViewModel
        {
            Date = today.AddDays(2),
            Time = new TimeOnly(20, 0, 0),
            People = 4,
            Phone = "5554999267175"
        };

        // Act
        var result = await service.MakeReservationAsync(model);

        // Assert
        Assert.That(result, !Is.Null);
        _appDbContextMock.Verify(context => context.Reservations.AddAsync(It.IsAny<Reservation>(), default), Times.Once);
        _appDbContextMock.Verify(context => context.SaveChangesAsync(default), Times.Once);
    }

    [Test]
    public async Task MakeReservationAsync_ReturnsNull_WhenInvalidRequest()
    {
        // Arrange
        var service = new RestaurantApplicationService(_appDbContextMock.Object);
        var model = new MakeReservationViewModel
        {
            Time = new TimeOnly(20, 0, 0),
            People = 4,
            Phone = "5554999267175"
        };

        // Act
        var result = await service.MakeReservationAsync(model);

        // Assert
        Assert.That(result, Is.Null);
        _appDbContextMock.Verify(context => context.Reservations.AddAsync(It.IsAny<Reservation>(), default), Times.Never);
        _appDbContextMock.Verify(context => context.SaveChangesAsync(default), Times.Never);
    }


    private IEnumerable<Reservation> CreateReservationsList()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        return new List<Reservation>
        {
            new() { Id = 1, Phone = "5554999267176", Date = new DateOnly(today.Year, today.Month, today.Day + 1), Time = new TimeOnly(19, 0, 0), People = 5, TableId = 1},
            new() { Id = 2, Phone = "5554999267175", Date = new DateOnly(today.Year, today.Month, today.Day - 1), Time = new TimeOnly(20, 0, 0), People = 5, TableId = 1},
            new() { Id = 3, Phone = "5554999267175", Date = today, Time = new TimeOnly(21, 0, 0), People = 4, TableId = 2}
        };
    }

    private IEnumerable<Table> CreateTableList()
    {
        return new List<Table>
        {
            new() { Id = 1, Reservations = CreateReservationsList().Where(res => res.People == 5).ToList(), Seats = 5 },
            new() { Id = 2, Reservations = CreateReservationsList().Where(res => res.People == 4).ToList(), Seats = 4 }
        };
    }
}