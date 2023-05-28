# UniBay.Result

Library for handling results, errors and  exceptions in a clean, consistent way across your applications.

The key ideas behind Result are:

- Standardize the error handling
- Throw exceptions only in exceptional scenarios
- Clear segregation between different types of errors

## Quick Start

Result provides usefull implicit conversions between different types what makes it really easy to work with.
Switching between standard approach and Result is effortles.
The only thing you have to do is wrapping return type into `Result<>` and replace `throw` with `return` keyword

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

ResultCode represents a status code. The package includes several predefined result codes that can be used out of the box:

```csharp
ResultCode.Ok
ResultCode.NoContent
ResultCode.Created
ResultCode.NotFound
ResultCode.Conflict
```

You can also create your own result codes by using  `ResultCode` class.


### Provided Exceptions

The package comes with predefined exceptions, which derive from `ExceptionBase`. Each exception maps to a specific `ResultCode`:

- `AuthorizationException`: Thrown when an authorization error occurs.
- `ConflictException`: Thrown when a conflict error occurs.
- `DomainException`: Thrown when a domain error occurs.
- `NotFoundException`: Thrown when a not found error occurs.
- `ServiceException`: Thrown when a service error occurs.
- `ValidationException`: Thrown when a validation error occurs.


### Aggregating Results

You can aggregate multiple `IResult` objects into a single `Result<Nothing>`. If all results are successful, an empty result with a status of `ResultCode.Ok` is returned. If any of the results fail, an `AggregateException` is returned containing all the exceptions:


```csharp
var result = ResultFactory.AggregatedResult(results);
```

## License

[MIT](LICENSE)