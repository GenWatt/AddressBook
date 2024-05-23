using AddressBook.Data;
using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Repository.AddressRepository;

public class AddressRepository : Repository<AddressModel>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AddressModel>> GetAllWithUser()
    {
        return await _context.Addresses.Include(address => address.User).ToListAsync();
    }

    public async Task<AddressModel?> GetByCity(string city)
    {
        return await _context.Addresses.FirstOrDefaultAsync(address => address.City == city);
    }
}


