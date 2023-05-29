using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UniBay.Result.AspNetCore.ActionResults;
using UniBay.Result.Exceptions;
using UniBay.Result.Types;

namespace UniBay.Result.AspNetCore;

public static class Extensions
{
    public static ValidationProblemDetails ToProblemDetails(this ModelStateDictionary dictionary)
        => new(dictionary)
        {
            Status = (int) ResultCode.ValidationFailed.HttpStatusCode
        };

    public static ValidationProblemDetails ToProblemDetails(this ValidationException e)
    {
        var problemDetails = e.ToModelStateDictionary().ToProblemDetails();
        problemDetails.Status = (int) ResultCode.ValidationFailed.HttpStatusCode;
        return problemDetails;
    }

    public static ModelStateDictionary ToModelStateDictionary(this ValidationException e)
    {
        var model = new ModelStateDictionary();
        model.AddValidationFailure(e.Failures.ToArray());
        return model;
    }

    public static void AddValidationFailure(this ModelStateDictionary model, IEnumerable<ValidationFailure> failures)
    {
        foreach (var failure in failures)
            model.AddModelError(failure.PropertyName, failure.Message);
    }

    public static IActionResult ToActionResult<TResult>(this Result<TResult> result)
        => new ResultActionResult<TResult>(result);
}