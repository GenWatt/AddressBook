using System.Security.Claims;
using AddressBook.DataTransferModels;
using AddressBook.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.ViewComponents;

public class AddAddressViewComponent : ViewComponent
{
    private readonly IUserService _userService;

    public AddAddressViewComponent(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync(AddressDTM address)
    {
        var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine(userId);
        address.Filter.UserId = userId;
        var addresses = await _userService.GetAllByFilter(address.Filter);
        address.UsersSuggestions = addresses;

        return View(address);
    }
}
