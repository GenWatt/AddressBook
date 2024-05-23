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

    private IQueryable<AddressModel> ApplyFilter(IQueryable<AddressModel> query, FilterDTM filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(address => filter.FilterBy == FilterBy.FirstName && address.User.FirstName.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.Surname && address.User.Surname.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.Email && address.User.Email.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.Phone && address.User.PhoneNumber.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.City && address.City.Contains(filter.Search));
        }

        return query;
    }

    public async Task<int> Count(FilterDTM filter)
    {
        var query = _context.Addresses.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = ApplyFilter(query, filter);
        }

        return await query.CountAsync();
    }

    public async Task<IEnumerable<AddressModel>> GetAllWithUser(FilterDTM filter)
    {
        var query = _context.Addresses
            .Include(address => address.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = ApplyFilter(query, filter);
        }

        return await query.Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<AddressModel?> GetByCity(string city)
    {
        return await _context.Addresses.FirstOrDefaultAsync(address => address.City == city);
    }
}


