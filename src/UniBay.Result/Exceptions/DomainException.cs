namespace UniBay.Result.Exceptions;

/// <summary>
/// Domain exception
/// For some business logic reason the operation could not be completed
/// The messageTemplate should be allowed to be seen by the end user
/// </summary>
public class DomainException : ResultException
{
    public DomainException(string errorCode, string messageTemplate, params object[] args)
            : base(code: ResultCode.DomainError,
                   errorCode: errorCode,
                   messageTemplate: messageTemplate,
                   args: args)
    {
    }

    public DomainException(Exception innerException, string errorCode, string messageTemplate, params object[] args)
            : base(innerException: innerException,
                   code: ResultCode.DomainError,
                   errorCode: errorCode,
                   messageTemplate: messageTemplate,
                   args: args)
    {
    }
}