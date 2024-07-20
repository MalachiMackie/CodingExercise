using CodingExercise.Api.Controllers;
using CodingExercise.Api.Models;
using CodingExercise.Domain.Models;
using CodingExercise.Domain.Models.ValueObjects;
using CodingExercise.Domain.Services;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace CodingExercise.Api.Tests;

public class PayslipControllerTests
{
    private readonly IPayslipGeneratorService _payslipGeneratorService = A.Fake<IPayslipGeneratorService>();
    private readonly PayslipController _payslipController;

    public PayslipControllerTests()
    {
        _payslipController = new PayslipController(_payslipGeneratorService);
        A.CallTo(() => _payslipGeneratorService.GeneratePayslip(An<IPayslipGeneratorService.EmployeeDetails>._))
            .Returns(Result.Ok(new IPayslipGeneratorService.GeneratedPayslip(
                DateRange.Create(
                    new DateOnly(2024, 07, 01),
                    new DateOnly(2024, 07, 31)).Value,
                1,
                2,
                3,
                4)));
    }
    
    [Fact]
    public void GeneratePayslip_Should_ReturnOk_When_ServiceReturnsSuccessful()
    {
        var result = _payslipController.GeneratePayslip(
            new GeneratePayslipRequest("FirstName", "LastName", 60_000, 0.25m));

        result.Should().BeConvertibleTo<OkObjectResult>()
            .And.Value.Should().BeEquivalentTo(
                new GeneratePayslipResponse(
                    new DateOnly(2024, 07, 01),
                    new DateOnly(2024, 07, 31),
                    1,
                    2,
                    3,
                    4));
    }

    [Fact]
    public void GeneratePayslip_Should_ReturnFailure_When_ServiceReturnsFailure()
    {
        A.CallTo(() => _payslipGeneratorService.GeneratePayslip(
                An<IPayslipGeneratorService.EmployeeDetails>._))
            .Returns(Result.NotFound<IPayslipGeneratorService.GeneratedPayslip>([]));

        var result = _payslipController.GeneratePayslip(
            new GeneratePayslipRequest("", "", 1, 1));

        result.Should().BeConvertibleTo<NotFoundObjectResult>();
    }
}