using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Reports.Application.Extensions;

/// <summary>
/// Provides extension methods for adding validation errors to the ModelStateDictionary.
/// </summary>
public static class AddToModelStateExtensions
{
    /// <summary>
    /// Adds the validation errors from the ValidationResult to the ModelStateDictionary.
    /// </summary>
    /// <param name="result">The ValidationResult containing the validation errors.</param>
    /// <param name="modelState">The ModelStateDictionary to add the errors to.</param>
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }

    /// <summary>
    /// Adds the errors from the Result to the ModelStateDictionary.
    /// </summary>
    /// <param name="result">The Result containing the errors.</param>
    /// <param name="modelState">The ModelStateDictionary to add the errors to.</param>
    public static void AddToModelState(this Result result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(string.Empty, error.Message);
        }
    }

    /// <summary>
    /// Adds the errors from the Result&lt;T&gt; to the ModelStateDictionary.
    /// </summary>
    /// <typeparam name="T">The type of the Result value.</typeparam>
    /// <param name="result">The Result&lt;T&gt; containing the errors.</param>
    /// <param name="modelState">The ModelStateDictionary to add the errors to.</param>
    public static void AddToModelState<T>(this Result<T> result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(string.Empty, error.Message);
        }
    }
}
