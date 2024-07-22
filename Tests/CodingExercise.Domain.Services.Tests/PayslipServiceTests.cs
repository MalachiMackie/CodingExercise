using CodingExercise.Domain.Models;
using CodingExercise.Domain.Models.ValueObjects;
using FakeItEasy;
using FluentAssertions;

namespace CodingExercise.Domain.Services.Tests;

public class PayslipServiceTests
{
    private static readonly DateTime Today = new (2023, 07, 22);
    private readonly PayslipGeneratorService _payslipGeneratorService;

    public PayslipServiceTests()
    {
        var timeProvider = A.Fake<TimeProvider>();
        A.CallTo(() => timeProvider.LocalTimeZone)
            .Returns(TimeZoneInfo.Utc);
        A.CallTo(() => timeProvider.GetUtcNow())
            .Returns(new DateTimeOffset(Today));
        _payslipGeneratorService = new PayslipGeneratorService(timeProvider);
    }

    [Fact]
    public void GeneratePayslip_Should_GenerateCorrectPayslip()
    {
        var result = _payslipGeneratorService.GeneratePayslip(
            new IPayslipGeneratorService.EmployeeDetails(
                "John",
                "Doe",
                60_050,
                9),
            "March");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new IPayslipGeneratorService.GeneratedPayslip(
            "John Doe",
            DateRange.Create(new DateOnly(Today.Year, 03, 1), new DateOnly(Today.Year, 03, 31)).Value,
            5004.17m,
            919.58m,
            4084.59m,
            450.38m));
    }
    
    [Fact]
    public void GeneratePayslip_Should_ReturnErrors_When_InputIsInvalid()
    {
        var employeeDetails = new IPayslipGeneratorService.EmployeeDetails(
            "",
            " ",
            -1,
            -1);

        var result = _payslipGeneratorService.GeneratePayslip(
            employeeDetails,
            "March");

        result.IsFailure.Should().BeTrue();
        result.FailureType.Should().Be(Result.Failure.InvalidInput);
        result.Errors.Should().BeEquivalentTo([
            "FirstName must not be empty",
            "LastName must not be empty",
            "AnnualSalary must be greater than $0",
            "SuperRate must be greater than or equal to 0%"
        ]);
    }

    [Fact]
    public void GeneratePayslip_Should_ReturnErrors_When_SuperIsAbove50Percent()
    {
        var employeeDetails = new IPayslipGeneratorService.EmployeeDetails(
            "",
            " ",
            -1,
            51);

        var result = _payslipGeneratorService.GeneratePayslip(
            employeeDetails,
            "March");

        result.IsFailure.Should().BeTrue();
        result.FailureType.Should().Be(Result.Failure.InvalidInput);
        result.Errors.Should().Contain("SuperRate must be at most 50%");
    }

    [Fact]
    public void GeneratePayslip_Should_AcceptSuperRateOfEqualTo50Percent()
    {
         var employeeDetails = new IPayslipGeneratorService.EmployeeDetails(
             "FirstName",
             "LastName",
             1000,
             50);
 
         var result = _payslipGeneratorService.GeneratePayslip(
             employeeDetails,
             "december");

         result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void GeneratePayslip_Should_ReturnFailure_When_MonthIsInvalid()
    {
        var result = _payslipGeneratorService.GeneratePayslip(
            new IPayslipGeneratorService.EmployeeDetails("First", "Last", 1000, 5),
            "invalid month");

        result.IsFailure.Should().BeTrue();
        result.FailureType.Should().Be(Result.Failure.InvalidInput);

    }
}