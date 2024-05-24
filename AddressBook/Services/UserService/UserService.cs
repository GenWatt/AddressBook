using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.UOW;
using AutoMapper;

namespace AddressBook.Services.UserService;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

    public async Task Update(UserDataPostDTM userData)
    {
        UserModel? userToUpdate = await _unitOfWork.userRepository.GetById(userData.Id);

        if (userToUpdate == null)
        {
            throw new Exception("User not found");
        }
        Console.WriteLine(userData.FirstName);
        _mapper.Map(userData, userToUpdate);
        Console.WriteLine(userToUpdate.FirstName);
        _unitOfWork.userRepository.Update(userToUpdate);
        await _unitOfWork.SaveChangesAsync();
    }
}