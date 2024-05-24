using AddressBook.Data;
using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Repository.UserRepository;

public class UserRepository : Repository<UserModel>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserModel?> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<IEnumerable<UserModel>> GetAllAddressesByUserId(string userId)
    {
        return await _context.Users
            .Include(user => user.Addresses)
            .Where(user => user.Id == userId)
            .ToListAsync();
    }

    public Task<UserModel?> GetById(string userId)
    {
        return _context.Users
            .Include(user => user.Addresses)
                .ThenInclude(address => address.User)
            .Include(user => user.Address)
            .FirstOrDefaultAsync(user => user.Id == userId);
    }
}