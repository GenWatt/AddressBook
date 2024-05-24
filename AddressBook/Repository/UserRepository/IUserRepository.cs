using AddressBook.DataTransferModels;
using AddressBook.Models;

namespace AddressBook.Repository.UserRepository;

public interface IUserRepository : IRepository<UserModel>
{
    // Returns all addresses of a user
    Task<IEnumerable<UserModel>> GetAllAddressesByUserId(string userId);
    Task<UserModel?> GetById(string userId);
    Task<IEnumerable<UserModel>> GetAllByFilter(FilterDTM filter);

    Task<int> CountByFilter(FilterDTM filter);
}