using Microsoft.Extensions.Logging;

namespace UniBay.Result.AspNetCore.ResponseMapper;

public static class ResultInspector
{
    private static readonly Dictionary<string, ResultLogger> resultCodeLoggers = new();
    internal static ILogger? Logger { get; set; }

    static ResultInspector()
    {
        LogFor(ResultCode.UnknownError, LogLevel.Error);
    }
    
    public static void LogFor(ResultCode code, LogLevel logLevel)
        => LogFor(code.Code, logLevel);
    public static void LogFor(string code, LogLevel logLevel)
    {
        if (ResultInspector.resultCodeLoggers.ContainsKey(code))
            ResultInspector.resultCodeLoggers.Remove(code);
        
        ResultInspector.resultCodeLoggers.Add(code, new ResultLogger(code, logLevel));
    }

    public static void Inspect(IResult result)
    {
        if (ResultInspector.Logger is null) return;
        if (result.IsSuccess) return;
        
        if (ResultInspector.resultCodeLoggers.TryGetValue(result.Code.Code, out var resultLogger))
        {
            resultLogger.Log(result, ResultInspector.Logger);
        }
    }
}