using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Data.Connection;

namespace RestaurantReservation.Data.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddSqlLiteContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
    }
}