namespace E_Commerce.Application.Common;

public class Result
{
    public bool IsSuccess { get; }

    public IReadOnlyList<Error> Errors { get; }

    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Ok()
    {
        return new(true, []);
    }

    public static Result Fail(Error error)
    {
        return new(false, [error]);
    }

    public static Result Fail(IReadOnlyList<Error> errors)
    {
        return new(false, errors);
    }

}

public class Result<TValue> : Result
{

    private readonly TValue _value;

    public TValue Data
    {
        get
        {
            return IsSuccess ? _value : throw new InvalidOperationException("Can Not Access The Value Of Failed Result");
        }
    }

    private Result(TValue value) : base(true, [])
    {
        _value = value;
    }

    private Result(Error error) : base(false, [error])
    {
        _value = default!;
    }

    private Result(IReadOnlyList<Error> errors) : base(false, errors)
    {
        _value = default!;
    }

    public static Result<TValue> Ok(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static new Result<TValue> Fail(Error error)
    {
        return new Result<TValue>(error);
    }

    public static new Result<TValue> Fail(IReadOnlyList<Error> errors)
    {
        return new Result<TValue>(errors);
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Ok(value);
    }

    public static implicit operator Result<TValue>(Error error)
    {
        return Fail(error);
    }

}
