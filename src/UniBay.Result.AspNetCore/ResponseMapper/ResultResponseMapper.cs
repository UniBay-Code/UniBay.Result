using UniBay.Result.AspNetCore.RequestResponses;
using UniBay.Result.Exceptions;

namespace UniBay.Result.AspNetCore.ResponseMapper;

public static class ResultResponseMapper
{
    private static readonly Dictionary<ResultCode, Func<IResult, object?>> mappers = new();

    static ResultResponseMapper()
    {
        RegisterErrorMapper(ResultCode.NotFound, result =>
        {
            if (result.Exception is NotFoundException e)
            {
                return new ResourceResponse(e.Resource);
            }

            return null;
        });

        RegisterErrorMapper(ResultCode.Conflict, result =>
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (result.Exception is ConflictException e)
            {
                return new ResourceResponse(e.Resource);
            }

            return null;
        });

        RegisterErrorMapper(ResultCode.ValidationFailed, result =>
        {
            if (result.Exception is ValidationException e)
            {
                return e.ToProblemDetails();
            }

            return null;
        });

        RegisterErrorMapper(ResultCode.DomainError, result =>
        {
            if (result.Exception is ResultException e)
            {
                return new ErrorResponse(e.Message, e.ErrorCode);
            }

            return null;
        });
    }

    /// <summary>
    /// Register mapping for error result code
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="mapper">Mapper</param>
    public static void RegisterErrorMapper(ResultCode code, Func<IResult, object?> mapper)
    {
        if (ResultResponseMapper.mappers.ContainsKey(code))
            ResultResponseMapper.mappers.Remove(code);

        ResultResponseMapper.mappers.Add(code, mapper);
    }

    /// <summary>
    /// Map result to object response
    /// </summary>
    /// <param name="result"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static object? Map<TResult>(Result<TResult> result)
    {
        if (ResultResponseMapper.mappers.TryGetValue(result.Code, out var mapper))
            return mapper(result);

        if (result.IsSuccess && typeof(TResult) != typeof(Nothing))
            return result.Value;

        return null;
    }
}