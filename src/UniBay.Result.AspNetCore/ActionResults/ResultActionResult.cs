using Microsoft.AspNetCore.Mvc;
using UniBay.Result.AspNetCore.ResponseMapper;

namespace UniBay.Result.AspNetCore.ActionResults;

public class ResultActionResult<TValue> : IActionResult
{
    public readonly int HttpStatusCode;
    private readonly Result<TValue> result;

    public ResultActionResult(Result<TValue> result)
    {
        this.result = result;
        this.HttpStatusCode = (int) result.Code.HttpStatusCode;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        IActionResult resultToExecute;
        ResultInspector.Inspect(this.result);
        var response = ResultResponseMapper.Map(this.result);
        if (response is null)
        {
            resultToExecute = new StatusCodeResult(this.HttpStatusCode);
        }
        else
        {
            resultToExecute = new ObjectResult(response)
            {
                StatusCode = this.HttpStatusCode
            };
        }

        await resultToExecute.ExecuteResultAsync(context);
    }

}