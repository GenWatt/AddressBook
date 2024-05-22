using AddressBook.Data;
using AddressBook.Repository.AddressRepository;

namespace AddressBook.UOW;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;

    public IAddressRepository addressRepository { get; protected set; }

    public UnitOfWork(ApplicationDbContext context, IAddressRepository addressRepository)
    {
        _context = context;
        this.addressRepository = addressRepository;
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

