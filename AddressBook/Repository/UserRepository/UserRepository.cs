using AddressBook.Data;
using AddressBook.DataTransferModels;
using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Repository.UserRepository;

public class UserRepository : Repository<UserModel>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    private IQueryable<UserModel> ApplyFilter(IQueryable<UserModel> query, FilterDTM filter)
    {
        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = query.Where(user => filter.FilterBy == FilterBy.FirstName && user.FirstName.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.Surname && user.Surname.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.Email && user.Email.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.Phone && user.PhoneNumber.Contains(filter.Search) ||
                 filter.FilterBy == FilterBy.City && user.Address.City.Contains(filter.Search));
        }

        return query;
    }

    private async Task<IQueryable<UserModel>> ApplayExludeIds(IQueryable<UserModel> query, FilterDTM filter)
    {
        var currentUser = await GetById(filter.UserId);
        var excludeUserIds = currentUser?.Users.Select(user => user.Id).ToList();
        return query.Where(user => excludeUserIds == null || !excludeUserIds.Contains(user.Id));
    }

    public async Task<int> CountByFilter(FilterDTM filter)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = ApplyFilter(query, filter);
        }

        query = await ApplayExludeIds(query, filter);

        return await query.CountAsync();
    }

    public async Task<IEnumerable<UserModel>> GetAllByFilter(FilterDTM filter)
    {
        var query = _context.Users
            .Include(user => user.Users)
            .Include(user => user.Address)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Search))
        {
            query = ApplyFilter(query, filter);
        }

        query = await ApplayExludeIds(query, filter);

        return await query.Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<UserModel?> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<IEnumerable<UserModel>> GetAllAddressesByUserId(string userId)
    {
        return await _context.Users
            .Include(user => user.Users)
            .Where(user => user.Id == userId)
            .ToListAsync();
    }

    public Task<UserModel?> GetById(string userId)
    {
        return _context.Users
            .Include(user => user.Users)
                .ThenInclude(address => address.Address)
            .Include(user => user.Address)
            .FirstOrDefaultAsync(user => user.Id == userId);
    }
}