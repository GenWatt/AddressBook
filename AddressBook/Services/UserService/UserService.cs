using AddressBook.Common;
using AddressBook.DataTransferModels;
using AddressBook.Models;
using AddressBook.Services.FileService;
using AddressBook.UOW;
using AutoMapper;

namespace AddressBook.Services.UserService;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _fileService = fileService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> AddAddressToUser(string userId, string userToAddId)
    {
        var userToAdd = await _unitOfWork.userRepository.GetById(userToAddId);
        if (userToAdd == null) return Result.Failure("User to add not found");

        var user = await _unitOfWork.userRepository.GetById(userId);
        if (user == null) return Result.Failure("User not found");

        user.Users.Add(userToAdd);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<IEnumerable<UserModel>>> GetAllAddressesByUserId(string userId)
    {
        var addresses = await _unitOfWork.userRepository.GetAllAddressesByUserId(userId);
        return Result<IEnumerable<UserModel>>.Success(data: addresses);
    }

    public async Task<Result<UserModel?>> GetById(string userId)
    {
        var user = await _unitOfWork.userRepository.GetById(userId);
        return user != null
            ? Result<UserModel?>.Success(data: user)
            : Result<UserModel?>.Failure("User not found");
    }

    public async Task<Result<int>> CountByFilter(FilterDTM filter)
    {
        var count = await _unitOfWork.userRepository.CountByFilter(filter);
        return Result<int>.Success(data: count);
    }

    public async Task<Result<IEnumerable<UserModel>>> GetAllByFilter(FilterDTM filterBy)
    {
        var users = await _unitOfWork.userRepository.GetAllByFilter(filterBy);
        return Result<IEnumerable<UserModel>>.Success(data: users);
    }

    public async Task<Result> DeleteAddressFormUser(string userId, string userToDeleteId)
    {
        var user = await _unitOfWork.userRepository.GetById(userId);
        if (user == null) return Result.Failure("User not found");

        var userToDelete = user.Users.FirstOrDefault(x => x.Id == userToDeleteId);
        if (userToDelete == null) return Result.Failure("User to delete not found");

        user.Users.Remove(userToDelete);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> Update(UserDataPostDTM userData)
    {
        var userToUpdate = await _unitOfWork.userRepository.GetById(userData.Id);
        if (userToUpdate == null) return Result.Failure("User not found");

        _mapper.Map(userData, userToUpdate);
        _unitOfWork.userRepository.Update(userToUpdate);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<UserDataDTM> PrepareUserDataDTM<T>(T userData)
    {
        var countryData = await _fileService.GetCountryData();
        var userDataDTM = _mapper.Map<UserDataDTM>(userData);
        userDataDTM.SelectData.CountryData = countryData;
        userDataDTM.CountryData = countryData;
        return userDataDTM;
    }
}