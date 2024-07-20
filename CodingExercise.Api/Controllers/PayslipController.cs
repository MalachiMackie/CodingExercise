using CodingExercise.Api.Models;
using CodingExercise.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingExercise.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PayslipController : CodingExerciseBaseController
{
    private readonly IPayslipGeneratorService _payslipGeneratorService;

    public PayslipController(IPayslipGeneratorService payslipGeneratorService)
    {
        _payslipGeneratorService = payslipGeneratorService;
    }

    /// <summary>
    /// Generate a payslip from the provided Employee Details
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("generate")]
    public ActionResult<GeneratePayslipResponse> GeneratePayslip([FromBody] GeneratePayslipRequest request)
    {
        var result = _payslipGeneratorService.GeneratePayslip(
            new IPayslipGeneratorService.EmployeeDetails(
                request.FirstName,
                request.LastName,
                request.AnnualSalary,
                request.SuperRatePercent));

        if (result.IsSuccess)
        {
            return MapPayslip(result.Value);
        }

        return MapResultFailureToActionResult(result);
    }

    private static GeneratePayslipResponse MapPayslip(IPayslipGeneratorService.GeneratedPayslip payslip)
    {
        return new GeneratePayslipResponse(
            payslip.DateRange.From,
            payslip.DateRange.To,
            payslip.GrossIncome,
            payslip.IncomeTax,
            payslip.NetIncome,
            payslip.Super);
    }
}
