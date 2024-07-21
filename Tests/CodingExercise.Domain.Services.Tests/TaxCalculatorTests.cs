using FluentAssertions;

namespace CodingExercise.Domain.Services.Tests;

public class TaxCalculatorTests
{
    [Fact]
    public void CalculateTax_Should_CalculateCorrectTaxForSalaryBelowFirstBracket()
    {
        const decimal annualSalary = 5010m;

        var result = TaxCalculator.CalculateIncomeTax(annualSalary);
        result.Should().Be(annualSalary * 0.105m);
    }

    [Fact]
    public void CalculateTax_Should_CalculateCorrectTaxForSalaryThatSpansMultipleBrackets()
    {
        const decimal annualSalary = 60_050;

        var result = TaxCalculator.CalculateIncomeTax(annualSalary);

        result.Should().Be(14000m * 0.105m + 34000m * 0.175m + 12050m * 0.3m);
    }

    [Fact]
    public void CalculateTax_Should_CalculateCorrectTaxForSalaryThatIsGreaterThanTheLastBracket()
    {
        const decimal annualSalary = 215_000;

        var result = TaxCalculator.CalculateIncomeTax(annualSalary);
        
        result.Should().Be(
            14_000 * 0.105m +
            34_000 * 0.175m +
            22_000 * 0.3m +
            110_000 * 0.33m +
            (annualSalary - 180_000) * 0.39m);
    }
}