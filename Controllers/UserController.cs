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

    public async Task<IActionResult> AddAddress(string userToAddId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        await _userService.AddAddressToUser(currentUserId, userToAddId);
        return RedirectToAction("Index", "Address");
    }

    public async Task<IActionResult> DeleteAddress(string userToDeleteId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        await _userService.DeleteAddressFormUser(currentUserId, userToDeleteId);
        return RedirectToAction("Index", "Address");
    }

    public async Task<IActionResult> Details(string userId)
    {
        var user = await _userService.GetById(userId);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
}