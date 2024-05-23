using AddressBook.Models;

namespace AddressBook.Repository.UserRepository;

public interface IUserRepository : IRepository<UserModel>
{
    Task<UserModel?> GetByEmail(string email);
}