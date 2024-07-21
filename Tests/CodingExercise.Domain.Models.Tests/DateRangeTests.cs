using CodingExercise.Domain.Models.ValueObjects;
using FluentAssertions;

namespace CodingExercise.Domain.Models.Tests;

public class DateRangeTests
{
    [Theory]
    [InlineData("2024-07-20", "2024-07-20")]
    [InlineData("2024-07-21", "2024-07-20")]
    public void Create_Should_ReturnInvalidInput_When_FromIsBeforeOrEqualTo(
        string fromString, string toString)
    {
        var fromDateOnly = DateOnly.Parse(fromString);
        var toDateOnly = DateOnly.Parse(toString);

        var result = DateRange.Create(fromDateOnly, toDateOnly);

        result.IsFailure.Should().BeTrue();
        result.FailureType.Should().Be(Result.Failure.InvalidInput);
    }

    [Fact]
    public void Create_Should_ReturnDateRange()
    {
        var from = new DateOnly(2024, 07, 20);
        var to = new DateOnly(2024, 07, 21);
        var result = DateRange.Create(
            from,
            to
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.From.Should().Be(from);
        result.Value.To.Should().Be(to);
    }

    [Theory]
    [InlineData("january", 2024, "2024-01-01", "2024-01-31")]
    [InlineData("January", 2024, "2024-01-01", "2024-01-31")]
    [InlineData("February", 2024, "2024-02-01", "2024-02-29")]
    [InlineData("February", 2023, "2023-02-01", "2023-02-28")]
    [InlineData("March", 2024, "2024-03-01", "2024-03-31")]
    [InlineData("April", 2024, "2024-04-01", "2024-04-30")]
    [InlineData("May", 2024, "2024-05-01", "2024-05-31")]
    [InlineData("June", 2024, "2024-06-01", "2024-06-30")]
    [InlineData("July", 2024, "2024-07-01", "2024-07-31")]
    [InlineData("August", 2024, "2024-08-01", "2024-08-31")]
    [InlineData("September", 2024, "2024-09-01", "2024-09-30")]
    [InlineData("October", 2024, "2024-10-01", "2024-10-31")]
    [InlineData("November", 2024, "2024-11-01", "2024-11-30")]
    [InlineData("December", 2024, "2024-12-01", "2024-12-31")]
    public void FromMonthAndYear_Should_SuccessfullyCreateDateRange(
        string month, int year, string startDateString, string endDateString)
    {
        var result = DateRange.FromMonthNameAndYear(month, year);
        var expected = DateRange.Create(
            DateOnly.Parse(startDateString),
            DateOnly.Parse(endDateString)).Value;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("some month", 2020)]
    [InlineData("March", -1)]
    [InlineData("March", 3001)]
    public void FromMonthAndYear_Should_ReturnFailure_When_InputIsInvalid(string monthName, int year)
    {
        var result = DateRange.FromMonthNameAndYear(monthName, year);
        result.IsFailure.Should().BeTrue();
        result.FailureType.Should().Be(Result.Failure.InvalidInput);
    }
    
}