using AddressBook.DataTransferModels;
using AddressBook.Models;

namespace AddressBook.Services.UserService;

public interface IUserService
{
    Task<UserModel?> GetById(string userId);
    Task<IEnumerable<UserModel>> GetAllAddressesByUserId(string userId);
    Task<IEnumerable<UserModel>> GetAllByFilter(FilterDTM filterBy);

    Task AddAddressToUser(string userId, string userToAddId);
    Task<int> CountByFilter(FilterDTM filter);

    Task DeleteAddressFormUser(string userId, string userToDeleteId);
    Task Update(UserDataPostDTM user);
}