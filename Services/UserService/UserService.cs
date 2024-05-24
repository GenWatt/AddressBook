using AddressBook.DataTransferModels;
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

    public async Task AddAddressToUser(string userId, string userToAddId)
    {
        var userToAdd = await _unitOfWork.userRepository.GetById(userToAddId);
        var user = await _unitOfWork.userRepository.GetById(userId);

        if (userToAdd == null)
        {
            throw new Exception("user to Add not found");
        }

        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.Users.Add(userToAdd);
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

    public async Task<int> CountByFilter(FilterDTM filter)
    {
        return await _unitOfWork.userRepository.CountByFilter(filter);
    }

    public async Task<IEnumerable<UserModel>> GetAllByFilter(FilterDTM filterBy)
    {
        return await _unitOfWork.userRepository.GetAllByFilter(filterBy);
    }

    public async Task DeleteAddressFormUser(string userId, string userToDeleteId)
    {
        var user = await _unitOfWork.userRepository.GetById(userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userToDelete = user.Users.FirstOrDefault(x => x.Id == userToDeleteId);

        if (userToDelete == null)
        {
            throw new Exception("user to delete not found");
        }

        user.Users.Remove(userToDelete);
        await _unitOfWork.SaveChangesAsync();
    }
}