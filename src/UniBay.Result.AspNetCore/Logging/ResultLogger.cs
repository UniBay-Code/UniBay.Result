using Microsoft.Extensions.Logging;
using UniBay.Result.Exceptions;

namespace UniBay.Result.AspNetCore.ResponseMapper;

internal readonly struct ResultLogger
{
    public string Code { get; }
    public LogLevel Level { get; }
    public ResultLogger(string code, LogLevel level)
    {
        this.Code = code;
        this.Level = level;
    }

    public void Log(IResult result, ILogger logger)
    {
        if (result.IsSuccess) return;
        if (result.Code.Code != this.Code) return;

        if (result.Exception is ResultException e)
        {
            logger.Log(this.Level, e, "Result with code {Code} was returned", result.Code.Code);
            return;
        }
        
        logger.Log(this.Level, result.Exception, "Result with code {Code} was returned", result.Code.Code);
    }
}