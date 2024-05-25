using AddressBook.Common;
using AddressBook.Models;
using AddressBook.Services.UserService;
using Moq;

public static class UserServiceMockHelper
{
    public static void SetupDeleteAddressFormUser(Mock<IUserService> userServiceMock)
    {
        userServiceMock.Setup(x => x.DeleteAddressFormUser(It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync(Result.Success());
    }

    public static void SetupGetById(Mock<IUserService> userServiceMock, UserModel userModel)
    {
        userServiceMock.Setup(x => x.GetById(It.IsAny<string>()))
                       .ReturnsAsync(Result<UserModel?>.Success(data: userModel));
    }

    public static void SetupAddAddressToUser(Mock<IUserService> userServiceMock)
    {
        userServiceMock.Setup(x => x.AddAddressToUser(It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync(Result.Success());
    }

    public static void SetupUpdate(Mock<IUserService> userServiceMock, UserModel userModel)
    {
        userServiceMock.Setup(x => x.GetById(It.IsAny<string>()))
                       .ReturnsAsync(Result<UserModel?>.Success(data: userModel));
    }
}