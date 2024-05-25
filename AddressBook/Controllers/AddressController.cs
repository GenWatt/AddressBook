using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;

namespace AddressBook.Controllers;

[EnableRateLimiting("fixed")]
[Authorize]
public class AddressController : BaseController
{
    private readonly IUserService _userService;

    public AddressController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var result = await _userService.GetById(userId);

        if (!result.IsSuccess) return Unauthorized();

        return HandleResult(result, () => View(result.Data));
    }

    public async Task<IActionResult> AddAddress(FilterDTM filter)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return Unauthorized();

        filter.UserId = currentUserId;
        var totalCount = await _userService.CountByFilter(filter);

        return View(new AddressDTM { Filter = filter, TotalCount = totalCount.Data });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

