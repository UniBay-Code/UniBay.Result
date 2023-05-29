using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UniBay.Result.AspNetCore.Middleware;
using UniBay.Result.AspNetCore.ResponseMapper;

namespace UniBay.Result.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IApplicationBuilder UseResultLogger(this IApplicationBuilder app)
    {
        var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Result");
        ResultInspector.Logger = logger;

        AddResultLoggers(app.ApplicationServices);
        
        return app;
    }
    
    public static IApplicationBuilder UseResultErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ResultErrorHandlingMiddleware>();
        return app;
    }

    private static void AddResultLoggers(IServiceProvider sp)
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection("ResultInspection");
        var config = new Dictionary<string, string>();
        section.Bind(config);

        foreach (var (key, value) in config)
        {
            if (Enum.TryParse<LogLevel>(value, true, out var logLevel))
            {
                ResultInspector.LogFor(key!, logLevel);
            }
        }
    }
}