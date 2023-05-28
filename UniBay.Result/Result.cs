using UniBay.Result.Exceptions;

namespace UniBay.Result;

public readonly struct Result<TType> : IResult<TType>
{
    public TType Value { get; }
    public ResultCode Code { get; }
    public Exception Exception { get; }

    public bool IsSuccess => this.Code.IsSuccess;
    public bool IsFailed => this.Code.IsFailed;

    private Result(TType value, ResultCode code)
    {
        this.Value = value;
        this.Code = code;
        this.Exception = null!;
    }
    private Result(TType value)
    {
        this.Value = value;
        this.Code = ResultCode.Ok;
        this.Exception = null!;

        if (value is Nothing)
        {
            this.Code = ResultCode.NoContent;
        }
    }

    internal Result(Exception exception, ResultCode? code = null)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        code ??= exception.GetResultCode();

        if (code.Value.IsSuccess)
        {
            throw new ArgumentException($"Invalid code value: {code}");
        }
        
        this.Code = code.Value;
        this.Exception = exception;
        this.Value = default!;
    }
    
    public static Result<TType>  Created(TType value) => new(value, ResultCode.Created);

    public static implicit operator Result<TType>(TType value) => new(value);
    public static implicit operator Result<TType>(Exception exception) => new(exception);
    public static implicit operator TType(Result<TType> result) => result.Value;

}
