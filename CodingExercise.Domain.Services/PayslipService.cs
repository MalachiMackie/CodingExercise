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
    /// <returns></returns>
    Result<GeneratedPayslip> GeneratePayslip(EmployeeDetails employeeDetails);

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
        DateRange DateRange,
        decimal GrossIncome,
        decimal IncomeTax,
        decimal NetIncome,
        decimal Super);
}

public class PayslipGeneratorService : IPayslipGeneratorService
{
    public Result<GeneratedPayslip> GeneratePayslip(EmployeeDetails employeeDetails)
    {
        throw new NotImplementedException();
    }
}