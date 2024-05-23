using AddressBook.DataTransferModels;
using AddressBook.Repository.UserRepository;
using AddressBook.Services.AddressService;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.ViewComponents;

public class AddAddressViewComponent : ViewComponent
{
    private readonly IAddressService _addressService;
    private readonly IUserRepository _userRepository;

    public AddAddressViewComponent(IAddressService addressService, IUserRepository userRepository)
    {
        _addressService = addressService;
        _userRepository = userRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync(AddressDTM address)
    {
        var addresses = await _addressService.GetAllWithUser(address.Filter);
        Console.WriteLine(address.TotalCount);
        address.UsersSuggestions = addresses;

        return View(address);
    }
}
