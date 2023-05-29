namespace UniBay.Result.AspNetCore.RequestResponses;

public readonly struct ErrorResponse
{
    /// <summary>
    /// Code of error
    /// </summary>
    /// <example>UserNotFound</example>
    public string Code { get; }

    /// <summary>
    /// Human-readable message
    /// </summary>
    /// <example>User with id '1' was not found.</example>
    public string Message { get; }

    public ErrorResponse(string message, string code)
    {
        this.Message = message;
        this.Code = code;
    }
}