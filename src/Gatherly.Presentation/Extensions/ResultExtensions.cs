using Gatherly.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Gatherly.Presentation.Extensions;

internal static class ResultExtensions
{
    internal static async Task<IActionResult> Match(
        this Task<Result> resultTask,
        Func<IActionResult> onSucces,
        Func<Result, IActionResult> onFailure)
    {
        Result result = await resultTask;

        return result.IsSuccess ? onSucces() : onFailure(result);
    }

    internal static async Task<IActionResult> Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, IActionResult> onSucces,
        Func<Result, IActionResult> onFailure)
    {
        Result<TIn> result = await resultTask;

        return result.IsSuccess ? onSucces(result.Value) : onFailure(result);
    }

}
