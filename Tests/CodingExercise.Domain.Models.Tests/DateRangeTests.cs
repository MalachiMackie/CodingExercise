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
}