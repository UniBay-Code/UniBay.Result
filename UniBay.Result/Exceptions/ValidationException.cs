using UniBay.Result.Types;

namespace UniBay.Result.Exceptions;

public class ValidationException : ResultException
{
    private readonly List<ValidationFailure> failures = new();
    public IReadOnlyCollection<ValidationFailure> Failures => this.failures.AsReadOnly();

    public ValidationException(ValidationFailure[] failures)
            : base(code: ResultCode.ValidationFailed,
                   messageTemplate: string.Join('\n', failures.Select(x => x.Message)))
    {
        this.failures.AddRange(failures);
    }

    public ValidationException(ValidationFailure failure)
            : this(new[] {failure})
    {
    }

    public ValidationException(string propertyName, string message)
            : this(new ValidationFailure(propertyName, message))
    {
    }


    /// <summary>
    /// Create exceptions as from parent property
    /// </summary>
    /// <param name="propertyName">name of parent property of validation exception</param>
    /// <returns></returns>
    public ValidationException FromParent(string propertyName)
        => new(this.failures.Select(x => x.FromParent(propertyName)).ToArray());
}