using System.Security.Claims;
using AddressBook.Common;
using AddressBook.Controllers;
using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services.UserService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AddressBook.Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task AddAddress_WhenUserIsAuthenticated_ShouldCallAddAddressToUserAndRedirectToIndex()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var validatorMock = new Mock<IValidator<UserDataPostDTM>>();
        UserServiceMockHelper.SetupAddAddressToUser(userServiceMock);

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "currentUserId")
        }, "mock"));

        var controller = UserControllerTestHelper.CreateUserControllerWithUserContext(userServiceMock, userClaims);

        // Act
        var result = await controller.AddAddress("userToAddId") as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Address", result.ControllerName);
        userServiceMock.Verify(x => x.AddAddressToUser("currentUserId", "userToAddId"), Times.Once);
    }

    [Fact]
    public async Task DeleteAddress_WhenUserIsAuthenticated_ShouldCallDeleteAddressFromUserAndRedirectToIndex()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        UserServiceMockHelper.SetupDeleteAddressFormUser(userServiceMock);
        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "currentUserId")
        }, "mock"));

        var controller = UserControllerTestHelper.CreateUserControllerWithUserContext(userServiceMock, userClaims);

        // Act
        var result = await controller.DeleteAddress("userToDeleteId") as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Address", result.ControllerName);
        userServiceMock.Verify(x => x.DeleteAddressFormUser("currentUserId", "userToDeleteId"), Times.Once);
    }

    [Fact]
    public async Task Details_WhenUserExists_ShouldReturnViewWithUser()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var validatorMock = new Mock<IValidator<UserDataPostDTM>>();
        var userModel = new UserModel { Id = "userId", FirstName = "John" };
        UserServiceMockHelper.SetupGetById(userServiceMock, userModel);

        var controller = new UserController(userServiceMock.Object, validatorMock.Object);

        // Act
        var result = await controller.Details("userId") as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserModel>(result.Model);
        var model = result.Model as UserModel;
        Assert.Equal("userId", model.Id);
        Assert.Equal("John", model.FirstName);
    }

    [Fact]
    public async Task Details_WhenUserDoesNotExist_ShouldReturnOtherErrorView()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var validatorMock = new Mock<IValidator<UserDataPostDTM>>();
        UserServiceMockHelper.SetupGetById(userServiceMock, null);

        var controller = new UserController(userServiceMock.Object, validatorMock.Object);

        // Act
        var result = await controller.Details("nonExistingUserId") as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("OtherError", result.ViewName);
        Assert.NotNull(result.ViewData["ErrorMessage"]);
    }

    [Fact]
    public async Task Update_WhenUserIsAuthenticated_ShouldReturnViewWithUserDataDTM()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var userModel = new UserModel { Id = "userId", FirstName = "John" };
        userServiceMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(Result<UserModel?>.Success("Success", userModel));

        var userDataDTM = new UserDataDTM { Id = "userId", FirstName = "John" };
        userServiceMock.Setup(x => x.PrepareUserDataDTM(userModel)).ReturnsAsync(userDataDTM);

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "currentUserId")
        }, "mock"));

        var controller = UserControllerTestHelper.CreateUserControllerWithUserContext(userServiceMock, userClaims);

        // Act
        var result = await controller.Update() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserDataDTM>(result.Model);
        var model = result.Model as UserDataDTM;
        Assert.Equal("userId", model.Id);
        Assert.Equal("John", model.FirstName);
    }
}

