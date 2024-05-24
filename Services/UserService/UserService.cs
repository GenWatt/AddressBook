using AddressBook.Models;
using AddressBook.UOW;

namespace AddressBook.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAddressToUser(string userId, int addressId)
    {
        var address = await _unitOfWork.addressRepository.GetById(addressId);
        var user = await _unitOfWork.userRepository.GetById(userId);

        if (address == null)
        {
            throw new Exception("Address not found");
        }

        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.Addresses.Add(address);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserModel>> GetAllAddressesByUserId(string userId)
    {
        return await _unitOfWork.userRepository.GetAllAddressesByUserId(userId);
    }

    public async Task<UserModel?> GetById(string userId)
    {
        return await _unitOfWork.userRepository.GetById(userId);
    }
}