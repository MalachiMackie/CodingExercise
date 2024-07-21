namespace CodingExercise.Domain.Models.ValueObjects;

// record so that we get equality by values, rather than reference equality
public record DateRange
{
    public DateOnly From { get; private init; }
    public DateOnly To { get; private init; }

    public static Result<DateRange> Create(DateOnly from, DateOnly to)
    {
        if (from >= to)
        {
            return Result.InvalidInput<DateRange>(["To date must be after from date"]);
        }

        return Result.Ok(new DateRange
        {
            From = from,
            To = to
        });
    }

    public static Result<DateRange> FromMonthNameAndYear(string monthName, int year)
    {
        if (year <= 1900)
        {
            return Result.InvalidInput<DateRange>(["Year must be greater than 1900"]);
        }

        if (year > 3000)
        {
            return Result.InvalidInput<DateRange>(["Year must be less than 3000"]);
        }

        if (!MonthNumbers.TryGetValue(monthName, out var monthNumber))
        {
            return Result.InvalidInput<DateRange>([$"{monthName} is not a valid month name"]);
        }

        var startOfMonth = new DateOnly(year, monthNumber, day: 1);

        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        return Create(startOfMonth, endOfMonth);
    }

    private static readonly Dictionary<string, int> MonthNumbers = new(
        StringComparer.InvariantCultureIgnoreCase)
    {
        { "january", 1 },
        { "february", 2 },
        { "march", 3 },
        { "april", 4 },
        { "may", 5 },
        { "june", 6 },
        { "july", 7 },
        { "august", 8 },
        { "september", 9 },
        { "october", 10 },
        { "november", 11 },
        { "december", 12 },
    };
}