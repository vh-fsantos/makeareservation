using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Data.Converters;

namespace RestaurantReservation.Application.Extensions;

public static class MvcOptionsExtension
{
    public static MvcOptions UseCustomConverters(this MvcOptions options)
    {
        TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
        TypeDescriptor.AddAttributes(typeof(TimeOnly), new TypeConverterAttribute(typeof(TimeOnlyTypeConverter)));
        return options;
    }
}