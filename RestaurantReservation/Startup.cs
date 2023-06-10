﻿using RestaurantReservation.Data.Extensions;

namespace RestaurantReservation;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSqlLiteContext();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllerRoute("default", "{controller=home}/{action=Index}/{id?}"));
    }
}