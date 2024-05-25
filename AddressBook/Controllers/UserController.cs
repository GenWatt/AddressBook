using AddressBook.DataTransferModels;
using AddressBook.Services.UserService;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AddressBook.Controllers;

[EnableRateLimiting("fixed")]
[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    private readonly IValidator<UserDataPostDTM> _validator;

    public UserController(IUserService userService, IValidator<UserDataPostDTM> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<IActionResult> AddAddress(string userToAddId)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return Unauthorized();

        var result = await _userService.AddAddressToUser(currentUserId, userToAddId);
        if (!result.IsSuccess) return HandleError(result);

        return HandleResult(result, () => RedirectToAction("Index", "Address"));
    }

    public async Task<IActionResult> DeleteAddress(string userToDeleteId)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return Unauthorized();


        var result = await _userService.DeleteAddressFormUser(currentUserId, userToDeleteId);
        if (!result.IsSuccess) return HandleError(result);

        return HandleResult(result, () => RedirectToAction("Index", "Address"));
    }

    public async Task<IActionResult> Details(string userId)
    {
        var result = await _userService.GetById(userId);
        if (!result.IsSuccess || result.Data == null) return HandleError(result);

        return HandleResult(result, () => View(result.Data));
    }

    public async Task<IActionResult> Update()
    {
        var currentUserId = GetCurrentUserId();
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        var result = await _userService.GetById(currentUserId);
        if (!result.IsSuccess || result.Data == null) return Unauthorized();

        var userDataDTM = await _userService.PrepareUserDataDTM(result.Data);
        return HandleResult(result, () => View(userDataDTM));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UserDataPostDTM userData)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return Unauthorized();

        var validationResult = _validator.Validate(userData);

        if (!validationResult.IsValid)
        {
            var userDataDTM = await _userService.PrepareUserDataDTM(userData);
            validationResult.AddToModelState(ModelState, null);
            return View(userDataDTM);
        }

        userData.Id = currentUserId;
        var result = await _userService.Update(userData);

        if (!result.IsSuccess) return HandleError(result);

        return HandleResult(result, () => RedirectToAction("Index", "Address"));
    }
}