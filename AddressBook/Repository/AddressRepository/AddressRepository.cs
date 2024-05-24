using AddressBook.Data;
using AddressBook.DataTransferModels;
using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Repository.AddressRepository;

public class AddressRepository : Repository<AddressModel>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<AddressModel?> GetByCity(string city)
    {
        return await _context.Addresses.FirstOrDefaultAsync(address => address.City == city);
    }
}


