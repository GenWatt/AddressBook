using System.Security.Claims;
using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services.FileService;
using AddressBook.Services.UserService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AddressBook.Controllers;

[EnableRateLimiting("fixed")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public UserController(IUserService userService, IMapper mapper, IFileService fileService)
    {
        _userService = userService;
        _mapper = mapper;
        _fileService = fileService;
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

    public async Task<IActionResult> Update()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var user = await _userService.GetById(currentUserId);

        if (user == null)
        {
            return NotFound();
        }

        var userDataDTM = _mapper.Map<UserDataDTM>(user);
        var countryData = await _fileService.GetCountryData();

        userDataDTM.SelectData.CountryData = countryData;
        userDataDTM.CountryData = countryData;

        return View(userDataDTM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UserDataPostDTM userData)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            var countryData = await _fileService.GetCountryData();

            return View();
        }

        userData.Id = currentUserId;

        await _userService.Update(userData);

        return RedirectToAction("Index", "Address");
    }
}