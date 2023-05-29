using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniBay.Result.AspNetCore.ResponseMapper;

namespace UniBay.Result.AspNetCore.Middleware;

public class ErrorHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new (JsonSerializerDefaults.Web);
    private readonly ILogger<ErrorHandlingMiddleware> logger;
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await this.next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, httpContext);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext httpContext)
    {
        Result<Nothing> result = exception;
        ResultInspector.Inspect(result);
        var response = ResultResponseMapper.Map(result);
        
        httpContext.Response.StatusCode = (int) result.Code.HttpStatusCode;
        httpContext.Response.ContentType = "application/json";
        
        var responsePayload = JsonSerializer.Serialize(response, ErrorHandlingMiddleware.SerializerOptions);
        await httpContext.Response.WriteAsync(responsePayload);
    }
}