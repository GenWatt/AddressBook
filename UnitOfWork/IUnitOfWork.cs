using AddressBook.Repository.AddressRepository;
using AddressBook.Repository.UserRepository;

namespace AddressBook.UOW
{
    public interface IUnitOfWork
    {
        IAddressRepository addressRepository { get; }
        IUserRepository userRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}
