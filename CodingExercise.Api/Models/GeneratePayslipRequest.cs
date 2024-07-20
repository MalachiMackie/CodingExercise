namespace CodingExercise.Api.Models;

public record GeneratePayslipRequest(
    string FirstName,
    string LastName,
    int AnnualSalary,
    decimal SuperRatePercent);

public record GeneratePayslipResponse(
    DateOnly PayPeriodFrom,
    DateOnly PayPeriodTo,
    decimal GrossIncome,
    decimal IncomeTax,
    decimal NetIncome,
    decimal Super);