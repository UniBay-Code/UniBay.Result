using System.Net;
using UniBay.Result.Exceptions;

namespace UniBay.Result.UnitTests.Exceptions;

public class ExceptionBaseTests
{
    [Fact]
    public void Given_CodeAndMessageAndArgs_When_ConstructingExceptionBase_Then_PropertiesAreSetCorrectly()
    {
        // Arrange
        var code = new ResultCode("TEST_CODE", false, HttpStatusCode.BadRequest);
        var message = "Test messageTemplate with argument: {0}";
        var arg = 42;

        // Act
        var exception = new ResultException(code, message, arg);

        // Assert
        Assert.Equal(code, exception.Code);
        Assert.Equal("TEST_CODE", exception.ErrorCode);
        Assert.Equal(message, exception.MessageTemplate);
        Assert.Equal($"Test messageTemplate with argument: {arg}", exception.Message);
        Assert.Equal(new object[] { arg }, exception.MessageArgs);
    }

    [Fact]
    public void Given_CodeAndErrorCodeAndMessageAndArgs_When_ConstructingExceptionBase_Then_PropertiesAreSetCorrectly()
    {
        // Arrange
        var code = new ResultCode("TEST_CODE", false, HttpStatusCode.BadRequest);
        var errorCode = "CUSTOM_ERROR_CODE";
        var message = "Test messageTemplate with argument: {0}";
        var arg = 42;

        // Act
        var exception = new ResultException(code, errorCode, message, arg);

        // Assert
        Assert.Equal(code, exception.Code);
        Assert.Equal(errorCode, exception.ErrorCode);
        Assert.Equal(message, exception.MessageTemplate);
        Assert.Equal($"Test messageTemplate with argument: {arg}", exception.Message);
        Assert.Equal(new object[] { arg }, exception.MessageArgs);
    }

    [Fact]
    public void Given_InnerExceptionAndCodeAndMessageAndArgs_When_ConstructingExceptionBase_Then_PropertiesAreSetCorrectly()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner exception");
        var code = new ResultCode("TEST_CODE", false, HttpStatusCode.BadRequest);
        var message = "Test messageTemplate with argument: {0}";
        var arg = 42;

        // Act
        var exception = new ResultException(innerException, code, message, arg);

        // Assert
        Assert.Equal(innerException, exception.InnerException);
        Assert.Equal(code, exception.Code);
        Assert.Equal("TEST_CODE", exception.ErrorCode);
        Assert.Equal(message, exception.MessageTemplate);
        Assert.Equal($"Test messageTemplate with argument: {arg}", exception.Message);
        Assert.Equal(new object[] { arg }, exception.MessageArgs);
    }

    [Fact]
    public void Given_InnerExceptionAndCodeAndErrorCodeAndMessageAndArgs_When_ConstructingExceptionBase_Then_PropertiesAreSetCorrectly()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner exception");
        var code = new ResultCode("TEST_CODE", false, HttpStatusCode.BadRequest);
        var errorCode = "CUSTOM_ERROR_CODE";
        var message = "Test messageTemplate with argument: {0}";
        var arg = 42;

        // Act
        var exception = new ResultException(innerException, code, errorCode, message, arg);

        // Assert
        Assert.Equal(innerException, exception.InnerException);
        Assert.Equal(code, exception.Code);
        Assert.Equal(errorCode, exception.ErrorCode);
        Assert.Equal(message, exception.MessageTemplate);
        Assert.Equal($"Test messageTemplate with argument: {arg}", exception.Message);
        Assert.Equal(new object[] { arg }, exception.MessageArgs);
    }
}
