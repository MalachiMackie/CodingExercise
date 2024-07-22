using CodingExercise.Api.Models;
using CodingExercise.Api.ViewModels;
using CodingExercise.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingExercise.Api.Controllers;

public class HomeController : Controller
{
    private readonly IPayslipGeneratorService _payslipGeneratorService;

    public HomeController(IPayslipGeneratorService payslipGeneratorService)
    {
        _payslipGeneratorService = payslipGeneratorService;
    }

    public IActionResult Index([FromForm] GeneratePayslipViewModel viewModel)
    {
        if (Request.Method == "GET")
        {
            return View(new GeneratePayslipViewModel());
        }

        var result = _payslipGeneratorService.GeneratePayslip(new IPayslipGeneratorService.EmployeeDetails(
                viewModel.FirstName,
                viewModel.LastName,
                viewModel.AnnualSalary,
                viewModel.SuperPercent),
            viewModel.Month);

        viewModel.Errors = result.Errors;
        if (result.IsSuccess)
        {
            viewModel.Response = new GeneratePayslipResponse(
                result.Value.FullName,
                result.Value.DateRange.From,
                result.Value.DateRange.To,
                result.Value.GrossIncome,
                result.Value.IncomeTax,
                result.Value.NetIncome,
                result.Value.Super);
        }
        
        return View(viewModel);
    }
}