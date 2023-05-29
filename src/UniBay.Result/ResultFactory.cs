using UniBay.Result.Exceptions;

namespace UniBay.Result;

public static class ResultFactory
{
    public static Result<Nothing> AggregatedResult(params IResult[] results)
    {
        var failedResults = results.Where(x => x.IsFailed).ToArray();
        if (!failedResults.Any()) return Nothing.Value;

        if (failedResults.All(x => x.Code == ResultCode.ValidationFailed))
        {
            var failures = failedResults
                          .Where(x => x.Exception is ValidationException)
                          .Select(x => x.Exception)
                          .Cast<ValidationException>()
                          .SelectMany(x => x.Failures);

            return new ValidationException(failures.ToArray());
        }

        return new AggregateResultException(failedResults.Select(x => x.Exception));
    }
}