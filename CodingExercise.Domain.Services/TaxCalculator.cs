namespace CodingExercise.Domain.Services;

public static class TaxCalculator
{
    private static readonly IReadOnlyCollection<TaxRange> TaxRanges = [
        new TaxRange(0, 14_000, 0.105m),
        new TaxRange(14_000, 48_000, 0.175m),
        new TaxRange(48_000, 70_000, 0.3m),
        new TaxRange(70_000, 180_000, 0.33m),
        new TaxRange(180_000, int.MaxValue, 0.39m)
    ];
    
    public static decimal CalculateIncomeTax(decimal annualSalary)
    {
        decimal incomeTax = 0;
        var applicableTaxRanges = TaxRanges.Where(x => annualSalary > x.From);
        foreach (var taxRange in applicableTaxRanges)
        {
            if (annualSalary > taxRange.To)
            {
                incomeTax += taxRange.AmountRange * taxRange.TaxPercent;
            }
            else
            {
                incomeTax += taxRange.TaxPercent * (annualSalary - taxRange.From);
            }
        }

        return Math.Round(incomeTax, 2);
    }

    private sealed record TaxRange(int From, int To, decimal TaxPercent)
    {
        public int AmountRange => To - From;
    }
}