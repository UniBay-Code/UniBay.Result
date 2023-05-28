namespace UniBay.Result.Exceptions;

/// <summary>
/// Aggregated exception for Result
/// </summary>
public class AggregateResultException : ResultException
{
    private readonly List<Exception> innerExceptions = new List<Exception>();
    public IReadOnlyCollection<Exception> InnerExceptions => this.innerExceptions.AsReadOnly();

    public AggregateResultException(IEnumerable<Exception> exceptions, string messageTemplate = "Many errors occurred")
            : base(ResultCode.Error, messageTemplate)
    {
        this.innerExceptions.AddRange(exceptions);
        this.Code = CalculateResultCode(exceptions);
    }

    public static ResultCode CalculateResultCode(IEnumerable<Exception> exceptions)
    {
        if (exceptions.All(x => x is ResultException))
        {
            var codes = exceptions
                       .Select(ex => ((ex as ResultException)!).Code)
                       .Distinct()
                       .ToArray();

            return codes.Length > 1 ? ResultCode.MultipleErrors : codes.First();
        }

        return ResultCode.UnknownError;
    }
}