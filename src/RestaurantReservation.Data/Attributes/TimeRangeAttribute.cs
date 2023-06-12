using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Data.Attributes;

public class TimeRangeAttribute : ValidationAttribute
{
    public TimeOnly Minimum { get; }
    public TimeOnly Maximum { get; }

    public TimeRangeAttribute(string minimum, string maximum)
    {
        Minimum = TimeOnly.Parse(minimum);
        Maximum = TimeOnly.Parse(maximum);
    }

    public override bool IsValid(object value)
    {
        if (value is TimeOnly time)
            return time >= Minimum && time <= Maximum;

        return false;
    }

    public override string FormatErrorMessage(string name) 
        => $"The field {name} must be inside the range {Minimum} - {Maximum}.";
}