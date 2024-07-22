namespace CodingExercise.Api.Models;

public record GeneratePayslipRequest(
    string FirstName,
    string LastName,
    int AnnualSalary,
    decimal SuperRatePercent,
    string MonthName,
    int Year);

public record GeneratePayslipResponse(
    string FullName,
    DateOnly PayPeriodFrom,
    DateOnly PayPeriodTo,
    decimal GrossIncome,
    decimal IncomeTax,
    decimal NetIncome,
    decimal Super);