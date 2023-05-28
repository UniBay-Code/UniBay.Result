using UniBay.Result.Exceptions;
using UniBay.Result.Types;

namespace UniBay.Result.UnitTests.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void GivenValidationFailuresArray_WhenCreatingValidationException_ThenFailuresAreAssigned()
    {
        // Arrange
        var failures = new[]
        {
            new ValidationFailure("property1", "message1"),
            new ValidationFailure("property2", "message2")
        };

        // Act
        var exception = new ValidationException(failures);

        // Assert
        Assert.Equal(failures.Length, exception.Failures.Count);
        Assert.True(failures.All(f => exception.Failures.Contains(f)));
    }

    [Fact]
    public void GivenValidationFailure_WhenCreatingValidationException_ThenFailuresContainSingleFailure()
    {
        // Arrange
        var failure = new ValidationFailure("property", "messageTemplate");

        // Act
        var exception = new ValidationException(failure);

        // Assert
        Assert.Single(exception.Failures, failure);
    }

    [Fact]
    public void GivenPropertyNameAndMessage_WhenCreatingValidationException_ThenFailuresContainSingleFailureWithSameData()
    {
        // Arrange
        var propertyName = "property";
        var message = "messageTemplate";

        // Act
        var exception = new ValidationException(propertyName, message);

        // Assert
        Assert.Single(exception.Failures);
        Assert.Equal(propertyName, exception.Failures.First().PropertyName);
        Assert.Equal(message, exception.Failures.First().Message);
    }

    [Fact]
    public void GivenParentPropertyName_WhenUsingFromParent_ThenFailuresAreUpdatedWithParentProperty()
    {
        // Arrange
        var parentPropertyName = "parent";
        var failure = new ValidationFailure("property", "messageTemplate");
        var exception = new ValidationException(failure);

        // Act
        var newException = exception.FromParent(parentPropertyName);

        // Assert
        Assert.Single(newException.Failures);
        Assert.Equal($"{parentPropertyName}.{failure.PropertyName}", newException.Failures.First().PropertyName);
        Assert.Equal(failure.Message, newException.Failures.First().Message);
    }
}