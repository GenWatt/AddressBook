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
}