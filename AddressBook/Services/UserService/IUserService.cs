using AddressBook.Common;
using AddressBook.DataTransferModels;
using AddressBook.Models;

namespace AddressBook.Services.UserService;

public interface IUserService
{
    Task<Result> AddAddressToUser(string userId, string userToAddId);

    Task<Result<IEnumerable<UserModel>>> GetAllAddressesByUserId(string userId);
    Task<Result<UserModel?>> GetById(string userId);
    Task<Result<int>> CountByFilter(FilterDTM filter);
    Task<Result<IEnumerable<UserModel>>> GetAllByFilter(FilterDTM filterBy);

    Task<Result> DeleteAddressFormUser(string userId, string userToDeleteId);
    Task<Result> Update(UserDataPostDTM userData);
    Task<UserDataDTM> PrepareUserDataDTM<T>(T userData);
}