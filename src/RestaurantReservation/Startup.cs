using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.Extensions;
using RestaurantReservation.Data.Extensions;

namespace RestaurantReservation;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options => options.UseCustomConverters());
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddSqlLiteContext();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllerRoute("default", "{controller=home}/{action=Index}/{id?}"));
    }
}