using UniBay.Result.Exceptions;

namespace UniBay.Result;

public static class ResultFactory
{
    public static Result<Nothing> AggregatedResult(params IResult[] results)
    {
        var failedResults = results.Where(x => x.IsFailed);
        if (!failedResults.Any()) return Nothing.Value;
        return new AggregateResultException(failedResults.Select(x => x.Exception));
    }
    
}