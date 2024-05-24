using AddressBook.Models;

namespace AddressBook.Repository.UserRepository;

public interface IUserRepository : IRepository<UserModel>
{
    // Returns all addresses of a user
    Task<IEnumerable<UserModel>> GetAllAddressesByUserId(string userId);
    Task<UserModel?> GetById(string userId);
}