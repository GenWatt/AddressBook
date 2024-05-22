using AddressBook.Repository.AddressRepository;

namespace AddressBook.UOW
{
    public interface IUnitOfWork
    {
        IAddressRepository addressRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}
