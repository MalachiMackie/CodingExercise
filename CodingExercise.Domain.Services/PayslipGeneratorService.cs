using CodingExercise.Domain.Models;
using CodingExercise.Domain.Models.ValueObjects;

using static CodingExercise.Domain.Services.IPayslipGeneratorService;

namespace CodingExercise.Domain.Services;

public interface IPayslipGeneratorService
{
    /// <summary>
    /// Generate a payslip from an Employee's Details
    /// </summary>
    /// <param name="employeeDetails"></param>
    /// <param name="monthName"></param>
    /// <returns></returns>
    Result<GeneratedPayslip> GeneratePayslip(EmployeeDetails employeeDetails, string monthName);

    /// <summary>
    /// Employee Details used to generate a payslip
    /// </summary>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="AnnualSalary"></param>
    /// <param name="SuperRatePercent"></param>
    record EmployeeDetails(string FirstName, string LastName, int AnnualSalary, decimal SuperRatePercent);

    /// <summary>
    /// A generated payslip from an employee's details
    /// </summary>
    record GeneratedPayslip(
        string FullName,
        DateRange DateRange,
        decimal GrossIncome,
        decimal IncomeTax,
        decimal NetIncome,
        decimal Super);
}

public class PayslipGeneratorService : IPayslipGeneratorService
{
    private readonly TimeProvider _timeProvider;

    public PayslipGeneratorService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public Result<GeneratedPayslip> GeneratePayslip(
        EmployeeDetails employeeDetails,
        string monthName)
    {
        var validationResult = Validate(employeeDetails);
        if (validationResult.IsFailure)
        {
            return Result.Fail<GeneratedPayslip>(validationResult);
        }
        
        var payPeriodRange = DateRange.FromMonthNameAndYear(monthName, _timeProvider.GetLocalNow().Year);
        if (payPeriodRange.IsFailure)
        {
            return Result.Fail<GeneratedPayslip>(payPeriodRange);
        }
        
        var yearlyIncomeTax = TaxCalculator.CalculateIncomeTax(employeeDetails.AnnualSalary);
        var monthlyIncomeTax = Math.Round(yearlyIncomeTax / 12m, 2);
        var grossIncome = Math.Round(employeeDetails.AnnualSalary / 12m, 2);
        var netIncome = grossIncome - monthlyIncomeTax;
        var superMultiplier = Math.Round(employeeDetails.SuperRatePercent * 0.01m, 2);
        var super = Math.Round(grossIncome * superMultiplier, 2);
        
        return Result.Ok(new GeneratedPayslip(
            $"{employeeDetails.FirstName} {employeeDetails.LastName}",
            payPeriodRange.Value,
            grossIncome,
            monthlyIncomeTax,
            netIncome,
            super));
    }
    
    private static Result Validate(EmployeeDetails employeeDetails)
    {
        List<string> errors = [];

        if (string.IsNullOrWhiteSpace(employeeDetails.FirstName))
        {
            errors.Add("FirstName must not be empty");
        }

        if (string.IsNullOrWhiteSpace(employeeDetails.LastName))
        {
            errors.Add("LastName must not be empty");
        }

        if (employeeDetails.AnnualSalary <= 0)
        {
            errors.Add("AnnualSalary must be greater than $0");
        }

        if (employeeDetails.SuperRatePercent < 0)
        {
            errors.Add("SuperRate must be greater than or equal to 0%");
        }
        else if (employeeDetails.SuperRatePercent > 50m)
        {
            errors.Add("SuperRate must be at most 50%");
        }

        if (errors.Count > 0)
        {
            return Result.InvalidInput(errors);
        }

        return Result.Ok();
    }
}