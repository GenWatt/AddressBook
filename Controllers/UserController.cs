using System.Security.Claims;
using AddressBook.Services;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> AddAddress(int addressId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        await _userService.AddAddressToUser(currentUserId, addressId);
        return RedirectToAction("Index", "Address");
    }
}