﻿using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.Extensions;
using RestaurantReservation.Data.Abstractions.Connection;
using RestaurantReservation.Data.Connection;
using RestaurantReservation.Data.Extensions;

namespace RestaurantReservation;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options => options.UseCustomConverters());
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddSqlLiteContext();
        services.AddScoped<IAppDbContext, AppDbContext>();
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