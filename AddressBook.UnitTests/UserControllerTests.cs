using System.Security.Claims;
using AddressBook.Controllers;
using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services.FileService;
using AddressBook.Services.UserService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        var fileServiceMock = new Mock<IFileService>();
        var autoMapperMock = new Mock<IMapper>();

        userServiceMock.Setup(x => x.AddAddressToUser(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        var controller = new UserController(userServiceMock.Object, autoMapperMock.Object, fileServiceMock.Object);
        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
              new Claim(ClaimTypes.NameIdentifier, "currentUserId")
        }, "mock"));

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = userClaims }
        };

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
        var fileServiceMock = new Mock<IFileService>();
        var autoMapperMock = new Mock<IMapper>();

        userServiceMock.Setup(x => x.AddAddressToUser(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        var controller = new UserController(userServiceMock.Object, autoMapperMock.Object, fileServiceMock.Object);
        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.NameIdentifier, "currentUserId")
            }, "mock"));

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = userClaims }
        };

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
        userServiceMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(new UserModel { Id = "userId", FirstName = "John" });

        var fileServiceMock = new Mock<IFileService>();
        var autoMapperMock = new Mock<IMapper>();
        var controller = new UserController(userServiceMock.Object, autoMapperMock.Object, fileServiceMock.Object);

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
    public async Task Details_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync((UserModel)null);

        var fileServiceMock = new Mock<IFileService>();
        var autoMapperMock = new Mock<IMapper>();
        var controller = new UserController(userServiceMock.Object, autoMapperMock.Object, fileServiceMock.Object);

        // Act
        var result = await controller.Details("nonExistingUserId") as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }
}

