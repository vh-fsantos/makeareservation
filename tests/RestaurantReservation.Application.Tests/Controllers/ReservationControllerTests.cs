using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RestaurantReservation.Application.ViewModels;
using RestaurantReservation.Data.Abstractions.Connection;
using RestaurantReservation.Domain.Models;

namespace RestaurantReservation.Application.Tests.Controllers;

[TestFixture]
public class ReservationControllerTests
{
    private Mock<IAppDbContext> _appDbContextMock;
    private Mock<DbSet<Reservation>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _appDbContextMock = new Mock<IAppDbContext>();
        _dbSetMock = new Mock<DbSet<Reservation>>();

        var reservations = new List<Reservation>
        {
            new() { Id = 1, Phone = "5554999267176", Date = new DateOnly(2023, 6, 20) },
            new() { Id = 2, Phone = "5554999267175", Date = new DateOnly(DateOnly.FromDateTime(DateTime.Now).Year - 1, 2, 1) },
            new() { Id = 3, Phone = "5554999267175", Date = new DateOnly(2023, 6, DateOnly.FromDateTime(DateTime.Now).Day - 1) }
        }.AsQueryable();

        _dbSetMock.As<IQueryable<Reservation>>().Setup(dbSet => dbSet.Provider).Returns(reservations.Provider);
        _dbSetMock.As<IQueryable<Reservation>>().Setup(dbSet => dbSet.Expression).Returns(reservations.Expression);
        _dbSetMock.As<IQueryable<Reservation>>().Setup(dbSet => dbSet.ElementType).Returns(reservations.ElementType);
        _dbSetMock.As<IAsyncEnumerable<Reservation>>().Setup(dbSet => dbSet).Returns(reservations);
        _appDbContextMock.Setup(context => context.Reservations).Returns(_dbSetMock.Object);
    }

    [Test]
    public async Task GetPastReservationsAsync_ReturnsReservations_WhenValidRequest()
    {
        // Arrange
        var controller = new ReservationController(_appDbContextMock.Object);
        var model = new GetPastReservationsViewModel
        {
            Phone = "5554999267176"
        };

        // Act
        var result = await controller.GetPastReservationsAsync(model);

        // Assert
        var teste = false;
    }
}