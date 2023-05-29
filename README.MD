# UniBay.Result

Library for handling results, errors and exceptions in a clean, consistent way across your applications.

The key ideas behind Result are:

- Standardize the error handling
- Throw exceptions only in exceptional scenarios
- Clear segregation between different types of errors

## Quick Start

Result provides useful implicit conversions between different types what makes it really easy to work with. Switching
between standard approach and Result is effortless. The only thing you have to do is wrapping return type
into `Result<>` and replace `throw` with `return` keyword

Before:

```csharp
public string GetSurname(string firstname)
{
    if (firstname != "John")
    {
        throw new ValidationException(nameof(firstname), "We provide surnames only for Johns.");
    }
    
    return "Doe";
}
```

After:

```csharp
public Result<string> GetSurname(string firstname)
{
    if (firstname != "John")
    {
        return new ValidationException(nameof(firstname), "We provide surnames only for Johns.");
    }
    
    return "Doe";
}
```

#### ResultCode

ResultCode represents a status code. The package includes several predefined result codes that can be used out of the
box:

```csharp
ResultCode.Ok
ResultCode.NoContent
ResultCode.Created
ResultCode.NotFound
ResultCode.Conflict
```

You can also create your own result codes by using  `ResultCode` class.

### Provided Exceptions

The package comes with predefined exceptions, which derive from `ExceptionBase`. Each exception maps to a
specific `ResultCode`:

- `AuthorizationException`: Thrown when an authorization error occurs.
- `ConflictException`: Thrown when a conflict error occurs.
- `DomainException`: Thrown when a domain error occurs.
- `NotFoundException`: Thrown when a not found error occurs.
- `ServiceException`: Thrown when a service error occurs.
- `ValidationException`: Thrown when a validation error occurs.

### Aggregating Results

You can aggregate multiple `IResult` objects into a single `Result<Nothing>`. If all results are successful, an empty
result with a status of `ResultCode.Ok` is returned. If any of the results fail, an `AggregateException` is returned
containing all the exceptions:

```csharp
var result = ResultFactory.AggregatedResult(results);
```

# UniBay.Result.AspNetCore

Set of useful tools for ASP .NET Framework which allows to:

- Inspect and log concrete result codes
- Translate Result into response objects based on different ResultCodes

### ResultActionResult

Library provides `ResultActionResult` which implements `IActionResult` interface.

```csharp
[Route("{id}")]
[HttpGet]
public async Task<IActionResult> GetBookById([FromRoute]int id)
{
    var request = new GetBookRequest(id);
    Result<Book> bookResult = await this.mediator.Send(request);
    return new ResultActionResult(bookResult);
}
```

### ResultResponseMapper

Sometimes for some concrete error types you want your application to return some specific structure to client.

`ResultActionResult` is using `ResultResponseMapper` to extract object response from `Result`.

- When `result.IsSuccess` and result type is not `Nothing` then `Result.Value` is returned
- When `result.IsSuccess` and result type is `Nothing` then null is returned
- When `result.IsFailed` then `ResultResponseMapper` is used.
- When `result.IsFailed` and `ResultResponseMapper` do not implements mapper for received `ResultCode` then null is
  returned

`ResultResponseMapper` by default maps few ResultCodes to responses:

- `ResultCode.NotFound` => `ResourceResponse` when exception is `NotFoundException`
- `ResultCode.Conflict` => `ResourceResponse` when exception is `ConflictException`
- `ResultCode.ValidationFailed` => `ValidationProblemDetails` when exception is `ValidationException`
- `ResultCode.DomainError` => `ErrorResponse` when exception is `ResultException`

`ResultResponseMapper` allow you to register your own mapping method for specific ResultCodes

```csharp
ResultResponseMapper.RegisterErrorMapper(ResultCode.UnhandledError, (result) =>
{
    ...
    ...
    ...
    
    return mappedResult;
});
```

Only one mapper per `ResultCode` can be registered. The last registered mapper is used.

### ResultInspector

Sometimes you do not expect to receive some ResultCode, but when it happens you want to know that.

ResultInspector is used to log information when that happens.

You can configure ResultInspector from code:

```csharp
ResultInspector.LogFor(ResultCode.UnknownError, LogLevel.Error);
```

or using `appsettings.json` file:

```json
{
  "ResultInspection": {
    "UnknownError": "Error",
    "ServiceError": "Warning"
  }
}
```

In order to make ResultInspector work you need to call

```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseResultLogger()
}
```

`ResultActionResult` use `ResultInspector` just before writing response.


## License

[MIT](LICENSE)