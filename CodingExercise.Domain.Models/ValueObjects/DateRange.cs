namespace CodingExercise.Domain.Models.ValueObjects;

// record so that we get equality by values, rather than reference equality
public record DateRange
{
    public DateOnly From { get; private init; }
    public DateOnly To { get; private init; }

    public static Result<DateRange> Create(DateOnly from, DateOnly to)
    {
        if (from <= to)
        {
            return Result.InvalidInput<DateRange>(["To date must be after from date"]);
        }

        return Result.Ok(new DateRange
        {
            From = from,
            To = to
        });
    }
}