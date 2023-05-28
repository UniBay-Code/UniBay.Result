using System.Net;
using UniBay.Result.Exceptions;

namespace UniBay.Result;

public static class Extensions
{
    public static ResultCode GetResultCode(this Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        ResultCode resultCode = exception switch
        {
            ResultException b     => b.Code,
            AggregateException ae => AggregateResultException.CalculateResultCode(ae.InnerExceptions),
            _                     => ResultCode.UnknownError
        };

        return resultCode;
    }


    /// <summary>
    /// In case if <see cref="ValidationException"/> occurs, sets parent property name
    /// </summary>
    public static Result<T> FromProperty<T>(this Result<T> result, string propertyName)
    {
        if (result.IsSuccess) return result;
        if (result.Exception is ValidationException ve)
        {
            return ve.FromParent(propertyName);
        }

        return result;
    }

    /// <summary>
    /// Converts successful result to created result
    /// </summary>
    public static Result<TValue> AsCreated<TValue>(this Result<TValue> result)
    {
        if (result.IsFailed) return result;
        if (typeof(TValue) == typeof(Nothing)) return result;

        return Result<TValue>.Created(result);
    }
}