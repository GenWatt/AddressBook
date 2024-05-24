using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;
using System.Security.Claims;

namespace AddressBook.Controllers;

[EnableRateLimiting("fixed")]
[Authorize]
public class AddressController : Controller
{
    private readonly IUserService _userService;

    public AddressController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userService.GetById(userId);

        if (user == null)
        {
            return Unauthorized();
        }

        return View(user);
    }

    public async Task<IActionResult> AddAddress(FilterDTM filter)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        filter.UserId = currentUserId;
        var totalCount = await _userService.CountByFilter(filter);
        return View(new AddressDTM { Filter = filter, TotalCount = totalCount });
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

