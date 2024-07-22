using CodingExercise.Api.Models;

namespace CodingExercise.Api.ViewModels;

public class GeneratePayslipViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int AnnualSalary { get; set; }
    public decimal SuperPercent { get; set; }
    public string Month { get; set; } = DateTime.Today.ToString("MMMM");
    public IReadOnlyCollection<string>? Errors { get; set; }
    public GeneratePayslipResponse? Response { get; set; }
}