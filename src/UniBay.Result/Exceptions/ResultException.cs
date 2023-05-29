namespace UniBay.Result.Exceptions;

/// <summary>
/// Base exception for Result usage
/// </summary>
public class ResultException : Exception
{
    /// <summary>
    /// The general error code for this exception.
    /// </summary>
    public ResultCode Code { get; protected set; }
    /// <summary>
    /// String error code, contains the unique error code.
    /// </summary>
    public string ErrorCode { get; }
    /// <summary>
    /// Template of error message
    /// </summary>
    public string? MessageTemplate { get; }
    /// <summary>
    /// Arguments of <see cref="MessageTemplate"/>
    /// </summary>
    public object[] MessageArgs { get; }
    

    /// <param name="code">Failed result code</param>
    /// <param name="errorCode">unique error code of the failure</param>
    /// <param name="messageTemplate">messageTemplate template</param>
    /// <param name="args">messageTemplate arguments</param>
    public ResultException(ResultCode code, string? errorCode, string? messageTemplate, params object[] args)
            : base(string.IsNullOrWhiteSpace(messageTemplate) ? string.Empty : string.Format(messageTemplate, args))
    {
        if (code.IsSuccess) throw new ArgumentOutOfRangeException($"{nameof(code)} cannot be success code.");
        
        this.Code = code;
        this.ErrorCode = errorCode ?? code.Code;
        this.MessageTemplate = messageTemplate;
        this.MessageArgs = args;
    }
    
    
    /// <param name="code">Failed result code</param>
    /// <param name="messageTemplate">messageTemplate template</param>
    /// <param name="args">messageTemplate arguments</param>
    public ResultException(ResultCode code, string? messageTemplate, params object[] args)
                    : this(code, code.Code, messageTemplate, args)
    {
    }
    
    
    /// <param name="innerException">Inner exception</param>
    /// <param name="code">Failed result code</param>
    /// <param name="errorCode">unique error code of the failure</param>
    /// <param name="messageTemplate">messageTemplate template</param>
    /// <param name="args">messageTemplate arguments</param>
    public ResultException(Exception innerException, ResultCode code, string? errorCode, string? messageTemplate, params object[] args)
            : base(string.IsNullOrWhiteSpace(messageTemplate) ? string.Empty : string.Format(messageTemplate, args), innerException)
    {
        if (code.IsSuccess) throw new ArgumentOutOfRangeException($"{nameof(code)} cannot be success code.");
        
        this.Code = code;
        this.ErrorCode = errorCode ?? code.Code;
        this.MessageTemplate = messageTemplate;
        this.MessageArgs = args;
    }
    
    /// <param name="innerException">Inner exception</param>
    /// <param name="code">Failed result code</param>
    /// <param name="messageTemplate">messageTemplate template</param>
    /// <param name="args">messageTemplate arguments</param>
    public ResultException(Exception innerException, ResultCode code, string? messageTemplate, params object[] args)
                    : this(innerException,code, code.Code, messageTemplate, args)
    {
    }

}