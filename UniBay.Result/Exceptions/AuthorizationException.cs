namespace UniBay.Result.Exceptions;

public class AuthorizationException : ResultException
{
    public AuthorizationException(string reason)
            : base(code: ResultCode.AuthorizationFailed,
                   messageTemplate: reason)

    {
    }
}