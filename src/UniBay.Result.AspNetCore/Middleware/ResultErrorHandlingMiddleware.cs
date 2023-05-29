using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniBay.Result.AspNetCore.ResponseMapper;

namespace UniBay.Result.AspNetCore.Middleware;

public class ResultErrorHandlingMiddleware
{
    private static readonly JsonSerializerOptions serializerOptions = new (JsonSerializerDefaults.Web);
    private readonly ILogger<ResultErrorHandlingMiddleware> logger;
    private readonly RequestDelegate next;

    public ResultErrorHandlingMiddleware(RequestDelegate next, ILogger<ResultErrorHandlingMiddleware> logger)
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
            this.logger.LogInformation("Handled exception: {Message}", ex.Message);
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
        
        var responsePayload = JsonSerializer.Serialize(response, ResultErrorHandlingMiddleware.serializerOptions);
        await httpContext.Response.WriteAsync(responsePayload);
    }
}