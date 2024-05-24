using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Services.AddressService;
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
    private readonly ILogger<AddressController> _logger;
    private readonly IAddressService _addressService;
    private readonly IUserService _userService;

    public AddressController(ILogger<AddressController> logger, IAddressService addressService, IUserService userService)
    {
        _logger = logger;
        _addressService = addressService;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAddress(int id)
    {
        await _addressService.Add(id);
        return RedirectToAction(nameof(Index));
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

