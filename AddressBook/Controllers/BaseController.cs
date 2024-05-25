using System.Security.Claims;
using AddressBook.Common;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers;

public abstract class BaseController : Controller
{
    protected async Task<IActionResult> HandleResult<T>(Result<T> result, Func<T, Task<IActionResult>> onSuccess)
    {
        if (result.IsSuccess)
        {
            return await onSuccess(result.Data);
        }
        else
        {
            ViewBag.ErrorMessage = result.Message;
            return View("OtherError");
        }
    }

    protected async Task<IActionResult> HandleResult(Result result, Func<Task<IActionResult>> onSuccess)
    {
        if (result.IsSuccess)
        {
            return await onSuccess();
        }
        else
        {
            ViewBag.ErrorMessage = result.Message;
            return View("OtherError");
        }
    }

    protected IActionResult HandleResult(Result result, Func<IActionResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess();
        }
        else
        {
            ViewBag.ErrorMessage = result.Message;
            return View("OtherError");
        }
    }

    protected IActionResult HandleError(Result result)
    {
        ViewBag.ErrorMessage = result.Message;
        return View("OtherError");
    }

    protected string? GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}