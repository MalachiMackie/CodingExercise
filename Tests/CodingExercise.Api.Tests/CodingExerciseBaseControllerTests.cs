using CodingExercise.Api.Controllers;
using CodingExercise.Domain.Models;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace CodingExercise.Api.Tests;

public class CodingExerciseBaseControllerTests
{
    private readonly TestController _testController = new();
    
    [Fact]
    public void NotFoundResultFailure_Should_MapToNotFoundObjectResult()
    {
        var result = Result.NotFound<string>(["my error"]);

        var actionResult = _testController.MapResult(result);

        actionResult.Should().BeNotFoundObjectResult()
            .ValueAs<IEnumerable<string>>()
            .Should().BeEquivalentTo(["my error"]);
    }

    [Fact]
    public void InvalidInputResultFailure_Should_MapToBadRequestObjectResult()
    {
        var result = Result.InvalidInput<string>(["invalid input"]);

        var actionResult = _testController.MapResult(result);

        actionResult.Should().BeBadRequestObjectResult()
            .ValueAs<IEnumerable<string>>()
            .Should().BeEquivalentTo(["invalid input"]);
    }

    [Fact]
    public void ErrorResultFailure_Should_MapToInternalServerErrorObjectResult()
    {
        var result = Result.Fail<string>(Result.Failure.Error, ["Something bad happened"]);

        var actionResult = _testController.MapResult(result);

        actionResult.Should().BeStatusCodeResult()
            .WithStatusCode(500);

        actionResult.Should().BeObjectResult()
            .ValueAs<IEnumerable<string>>()
            .Should().BeEquivalentTo(["Something bad happened"]);
    }

    [Fact]
    public void SuccessfulResult_Should_ThrowBadException()
    {
        var result = Result.Ok("my value");

        _testController.Invoking(x => x.MapResult(result))
            .Should().Throw<InvalidOperationException>();
    }
}

public class TestController : CodingExerciseBaseController
{
    public ActionResult MapResult(Result result)
    {
        return MapResultFailureToActionResult(result);
    }
}