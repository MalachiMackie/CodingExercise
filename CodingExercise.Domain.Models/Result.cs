namespace CodingExercise.Domain.Models;

public class Result
{
    public enum Failure
    {
        InvalidInput,
        NotFound,
        Error
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Failure? FailureType { get; }
    public IReadOnlyCollection<string> Errors { get; } = [];

    public Result(bool isSuccess, Failure? failureType, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        FailureType = failureType;
        Errors = errors.ToArray();
    }
    
    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value);
    }

    public static Result Ok()
    {
        return new Result(true, null, []);
    }

    public static Result InvalidInput(IEnumerable<string> errors)
    {
        return new Result(false, Failure.InvalidInput, errors);
    }

    public static Result<T> InvalidInput<T>(IEnumerable<string> errors)
    {
        return new Result<T>(Failure.InvalidInput, errors);
    }

    public static Result<T> Fail<T>(Failure failureType, IEnumerable<string> errors)
    {
        return new Result<T>(failureType, errors);
    }
    
    public static Result<T> Fail<T>(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Expected a failed result");
        }
        
        return new Result<T>(result.FailureType!.Value, result.Errors);
    }

    public static Result<T> NotFound<T>(IEnumerable<string> errors)
    {
        return new Result<T>(Failure.NotFound, errors);
    }
}

public class Result<T> : Result
{
    public Result(T value) : base(isSuccess: true, failureType: null, [])
    {
        _value = value;
    }

    public Result(Failure failureType, IEnumerable<string> errors) : base(isSuccess: false, failureType, errors)
    {
        _value = default;
    }

    private readonly T? _value;

    public T Value => _value ?? throw new InvalidOperationException("Cannot access value of a failed Result");
}