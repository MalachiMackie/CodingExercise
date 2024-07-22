using System.Net.Http.Json;
using CodingExercise.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace CodingExercise.IntegrationTests;

public class GeneratePayslipTests
{
    private readonly HttpClient _httpClient;

    public GeneratePayslipTests()
    {
        var applicationFactory = new TestWebApplicationFactory<Program>();
        _httpClient = applicationFactory.CreateClient();
    }
    
    [Fact]
    public async Task GeneratePayslip_Should_GeneratePayslipCorrectly()
    {
        var query = new QueryString()
            .Add("FirstName", "Malachi")
            .Add("LastName", "Mackie")
            .Add("AnnualSalary", "60050")
            .Add("SuperRatePercent", "9")
            .Add("MonthName", "March")
            .Add("Year", "2024");
        
        var response = await _httpClient.GetAsync($"/api/Payslip/generate{query.Value}");

        response.Should().BeSuccessful();
        var payslipResponse = await response.Content.ReadFromJsonAsync<GeneratePayslipResponse>();
        payslipResponse.Should().BeEquivalentTo(new GeneratePayslipResponse(
            "Malachi Mackie",
            new DateOnly(2024, 03, 01),
            new DateOnly(2024, 03, 31),
            5004.17m,
            919.58m,
            4084.59m,
            450.38m
        ));
    }
}