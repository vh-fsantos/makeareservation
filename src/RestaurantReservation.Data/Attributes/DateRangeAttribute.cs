using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Data.Attributes;

public class DateRangeAttribute : ValidationAttribute
{
    public DateOnly Minimum { get; }
    public DateOnly Maximum { get; }

    public DateRangeAttribute(string minimum, string maximum)
    {
        Minimum = DateOnly.Parse(minimum);
        Maximum = DateOnly.Parse(maximum);
    }

    public override bool IsValid(object value)
    {
        if (value is DateOnly date)
            return date >= Minimum && date <= Maximum;

        return false;
    }

    public override string FormatErrorMessage(string name)
        => $"The field {name} must be inside the range {Minimum} - {Maximum}.";
}