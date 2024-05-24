using AddressBook.Data;
using AddressBook.Repository.AddressRepository;
using AddressBook.Repository.UserRepository;

namespace AddressBook.UOW;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;

    public IAddressRepository addressRepository { get; protected set; }
    public IUserRepository userRepository { get; protected set; }

    public UnitOfWork(ApplicationDbContext context, IAddressRepository addressRepository, IUserRepository userRepository)
    {
        _context = context;
        this.addressRepository = addressRepository;
        this.userRepository = userRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        return await _context.SaveChangesAsync(token);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

