using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Reports.Application.Validators;

public static class AddToModelStateExtentions
{
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }

    public static void AddToModelState(this Result result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(string.Empty, error.Message);
        }
    }

    public static void AddToModelState<T>(this Result<T> result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(string.Empty, error.Message);
        }
    }
}
