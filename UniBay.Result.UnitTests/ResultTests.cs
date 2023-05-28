using FluentAssertions;
using UniBay.Result.Exceptions;
using UniBay.Result.Types;

namespace UniBay.Result.UnitTests;

public class ResultTests
{
    [Fact]
    public void GivenSuccessfulResult_WhenAsCreatedCalled_ThenResultShouldBeCreated()
    {
        // Arrange
        var successResult = Result<string>.Created("Hello");

        // Act
        var createdResult = successResult.AsCreated();

        // Assert
        createdResult.IsSuccess.Should().BeTrue();
        createdResult.Code.Should().Be(ResultCode.Created);
        createdResult.Value.Should().Be("Hello");
    }

    [Fact]
    public void GivenFailedResult_WhenAsCreatedCalled_ThenResultShouldBeSame()
    {
        // Arrange
        var exception = new DomainException("ERROR1", "Sample error");
        Result<string> failedResult = exception;

        // Act
        var unchangedResult = failedResult.AsCreated();

        // Assert
        unchangedResult.Should().Be(failedResult);
    }

    [Fact]
    public void GivenSuccessfulResult_WhenFromPropertyCalled_ThenResultShouldBeSame()
    {
        // Arrange
        var successResult = Result<string>.Created("Hello");

        // Act
        var unchangedResult = successResult.FromProperty("TestProperty");

        // Assert
        unchangedResult.Should().Be(successResult);
    }

    [Fact]
    public void GivenFailedResultWithValidationException_WhenFromPropertyCalled_ThenResultShouldBeChanged()
    {
        // Arrange
        var exception = new ValidationException("propertyName", "value is invalid");
        Result<string> failedResult = exception;

        // Act
        var changedResult = failedResult.FromProperty("TestProperty");

        // Assert
        changedResult.Should().NotBe(failedResult);
        changedResult.Exception.Should().BeOfType<ValidationException>();
        var failures = ((ValidationException) changedResult.Exception).Failures;
        failures.Select(x => x.PropertyName).Should().AllSatisfy(propertyName => propertyName.Should().StartWith("TestProperty"));
    }

    [Fact]
    public void GivenFailedResultWithoutValidationException_WhenFromPropertyCalled_ThenResultShouldBeSame()
    {
        // Arrange
        var exception = new DomainException("ERROR1", "Sample error");
        Result<string> failedResult = exception;

        // Act
        var unchangedResult = failedResult.FromProperty("TestProperty");

        // Assert
        unchangedResult.Should().Be(failedResult);
    }

    [Fact]
    public void GivenMultipleSuccessfulResults_WhenAggregatedResultCalled_ThenResultShouldBeSuccessful()
    {
        // Arrange
        var successResult1 = Result<string>.Created("Hello");
        var successResult2 = Result<string>.Created("World");

        // Act
        var aggregatedResult = ResultFactory.AggregatedResult(successResult1, successResult2);

        // Assert
        aggregatedResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void GivenMixedResults_WhenAggregatedResultCalled_ThenResultShouldBeFailed()
    {
        // Arrange
        var successResult = Result<string>.Created("Hello");
        var exception = new DomainException("ERROR1", "Sample error");
        Result<string> failedResult = exception;

        // Act
        var aggregatedResult = ResultFactory.AggregatedResult(successResult, failedResult);

        // Assert
        aggregatedResult.IsFailed.Should().BeTrue();
        aggregatedResult.Exception.Should().BeOfType<AggregateResultException>();
        ((AggregateResultException) aggregatedResult.Exception).InnerExceptions.Should().Contain(exception);
    }

    [Fact]
    public void GivenExceptionBase_WhenResultIsCreated_ThenResultCodeShouldBeSetAccordingly()
    {
        // Arrange
        var exception = new ResultException(ResultCode.DomainError, "Sample error");

        // Act
        Result<string> result = exception;

        // Assert
        result.Code.Should().Be(ResultCode.DomainError);
    }

    [Fact]
    public void GivenAuthorizationException_WhenResultIsCreated_ThenResultCodeShouldBeAuthorizationFailed()
    {
        // Arrange
        var exception = new AuthorizationException("Unauthorized access");

        // Act
        Result<string> result = exception;

        // Assert
        result.Code.Should().Be(ResultCode.AuthorizationFailed);
    }

    [Fact]
    public void GivenConflictException_WhenResultIsCreated_ThenResultCodeShouldBeConflict()
    {
        // Arrange
        var exception = ConflictException.Create<AppResource>("1");

        // Act
        Result<string> result = exception;

        // Assert
        result.Code.Should().Be(ResultCode.Conflict);
    }

    [Fact]
    public void GivenDomainException_WhenResultIsCreated_ThenResultCodeShouldBeDomainError()
    {
        // Arrange
        var exception = new DomainException("ERROR1", "Sample error");

        // Act
        Result<string> result = exception;

        // Assert
        result.Code.Should().Be(ResultCode.DomainError);
    }

    [Fact]
    public void GivenNotFoundException_WhenResultIsCreated_ThenResultCodeShouldBeNotFound()
    {
        // Arrange
        var exception = NotFoundException.Create<AppResource>("1");

        // Act
        Result<string> result = exception;

        // Assert
        result.Code.Should().Be(ResultCode.NotFound);
    }

    [Fact]
    public void GivenServiceException_WhenResultIsCreated_ThenResultCodeShouldBeServiceError()
    {
        // Arrange
        var exception = new ServiceException("Internal error");

        // Act
        Result<string> result = exception;

        // Assert
        result.Code.Should().Be(ResultCode.ServiceError);
    }

    [Fact]
    public void GivenRegularException_WhenResultIsCreated_ThenResultCodeShouldBeUnknownError()
    {
        // Arrange
        var exception = new Exception("Regular exception");

        // Act
        Result<string> result = exception;

        // Assert
        result.Code.Should().Be(ResultCode.UnknownError);
    }

    [Fact]
    public void GivenAggregateExceptionWithMixedExceptions_WhenResultIsCreated_ThenResultCodeShouldBeUnknownError()
    {
        // Arrange
        var exception1 = new DomainException("ERROR1", "Sample error");
        var exception2 = new Exception("Regular exception");
        var aggregateException = new AggregateException(exception1, exception2);

        // Act
        Result<string> result = aggregateException;

        // Assert
        result.Code.Should().Be(ResultCode.UnknownError);
    }

    [Fact]
    public void GivenAggregateExceptionWithUniformExceptions_WhenResultIsCreated_ThenResultCodeShouldBeSameForAll()
    {
        // Arrange
        var exception1 = new DomainException("ERROR1", "Sample error");
        var exception2 = new DomainException("ERROR2", "Another sample error");
        var aggregateException = new AggregateException(exception1, exception2);

        // Act
        Result<string> result = aggregateException;

        // Assert
        result.Code.Should().Be(ResultCode.DomainError);
    }

    [Fact]
    public void GivenAggregateExceptionWithDifferentExceptionBaseTypes_WhenResultIsCreated_ThenResultCodeShouldBeMultipleErrors()
    {
        // Arrange
        var exception1 = new DomainException("ERROR1", "Sample error");
        var exception2 = new ServiceException("Service error");
        var aggregateException = new AggregateException(exception1, exception2);

        // Act
        Result<string> result = aggregateException;

        // Assert
        result.Code.Should().Be(ResultCode.MultipleErrors);
    }

    [Fact]
    public void GivenAggregateExceptionWithUniformExceptionBaseTypesButDifferentCodes_WhenResultIsCreated_ThenResultCodeShouldBeMultipleErrors()
    {
        // Arrange
        var exception1 = new DomainException("ERROR1", "Sample error");
        var exception2 = new DomainException("ERROR2", "Another sample error");
        var exception3 = new ServiceException("Service error");
        var aggregateException = new AggregateException(exception1, exception2, exception3);

        // Act
        Result<string> result = aggregateException;

        // Assert
        result.Code.Should().Be(ResultCode.MultipleErrors);
    }
}