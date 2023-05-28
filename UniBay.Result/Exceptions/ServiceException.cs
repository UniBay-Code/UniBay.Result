namespace UniBay.Result.Exceptions;

/// <summary>
/// Exception should be returned when API was misused or developer made a mistake.
/// When presentation layer and api is written correctly - this messageTemplate should never be seen by end user.
/// When such exception occurs, the developer need to fix code or add some additional validation in order to prevent this exception be returned again
/// </summary>
public class ServiceException : ResultException
{
    public ServiceException(string messageTemplate, params object[] args)
            : base(code: ResultCode.ServiceError,
                   messageTemplate: messageTemplate,
                   args: args)
    {
    }


    public ServiceException(Exception innerException, string messageTemplate, params object[] args)
            : base(innerException: innerException,
                   code: ResultCode.ServiceError,
                   messageTemplate: messageTemplate,
                   args: args)
    {
    }
}