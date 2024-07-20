using System.Diagnostics;
using CodingExercise.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingExercise.Api.Controllers;

public class CodingExerciseBaseController : ControllerBase
{
    protected ActionResult MapResultFailureToActionResult(Result result)
    {
        return result switch
        {
            { IsFailure: true, FailureType: Result.Failure.InvalidInput } => BadRequest(result.Errors),
            { IsFailure: true, FailureType: Result.Failure.NotFound } => NotFound(result.Errors),
            { IsFailure: true} => StatusCode(500, result.Errors),
            { IsSuccess: true } => throw new InvalidOperationException("Expected failed result"),
            _ => throw new UnreachableException("IsSuccess and IsFailure are mutually exclusive")
        };
    }
}