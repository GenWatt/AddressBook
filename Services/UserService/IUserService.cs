using AddressBook.Models;

namespace AddressBook.Services;

public interface IUserService
{
    Task<UserModel?> GetById(string userId);
    Task<IEnumerable<UserModel>> GetAllAddressesByUserId(string userId);

    Task AddAddressToUser(string userId, int addressId);
}