using System.Security.Claims;
using AddressBook.Controllers;
using AddressBook.DataTransferModels;
using AddressBook.Services.UserService;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

public static class UserControllerTestHelper
{
    public static UserController CreateUserControllerWithUserContext(Mock<IUserService> userServiceMock, ClaimsPrincipal userClaims)
    {
        var validatorMock = new Mock<IValidator<UserDataPostDTM>>();
        var controller = new UserController(userServiceMock.Object, validatorMock.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userClaims }
            }
        };

        return controller;
    }
}