using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services.AddressService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AddressBook.Controllers;

public class AddressController : Controller
{
    private readonly ILogger<AddressController> _logger;
    private readonly IAddressService _addressService;

    public AddressController(ILogger<AddressController> logger, IAddressService addressService)
    {
        _logger = logger;
        _addressService = addressService;
    }

    public async Task<IActionResult> Index()
    {
        var addresses = await _addressService.GetAllWithUser(new FilterDTM());
        return View(addresses);
    }

    public async Task<IActionResult> AddAddress(FilterDTM filter)
    {
        var totalCount = await _addressService.Count(filter);
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

