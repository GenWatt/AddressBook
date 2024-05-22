using AddressBook.Models;
using AddressBook.UOW;

namespace AddressBook.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly IUnitOfWork _unitOfWork;

    public AddressService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add(AddressModel address)
    {
        _unitOfWork.addressRepository.Insert(address);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        _unitOfWork.addressRepository.Delete(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public Task Delete(AddressModel address)
    {
        _unitOfWork.addressRepository.Delete(address);
        return _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<AddressModel>> GetAll()
    {
        return await _unitOfWork.addressRepository.GetAll();
    }

    public async Task<AddressModel?> GetByCity(string city)
    {
        return await _unitOfWork.addressRepository.GetByCity(city);
    }

    public async Task<AddressModel?> GetById(int id)
    {
        return await _unitOfWork.addressRepository.GetById(id);
    }

    public async Task<AddressModel?> GetById(AddressModel address)
    {
       return await _unitOfWork.addressRepository.GetById(address);
    }

    public async Task Update(AddressModel address)
    {
        var existingAddress = await _unitOfWork.addressRepository.GetById(address.Id);

        if (existingAddress == null)
        {
            throw new Exception("Address not found");
        }

        existingAddress.Street = address.Street;
        existingAddress.City = address.City;
        existingAddress.Zip = address.Zip;
        existingAddress.Country = address.Country;
        existingAddress.UpdatedAt = DateTime.Now;

        _unitOfWork.addressRepository.Update(existingAddress);
        await _unitOfWork.SaveChangesAsync();
    }
}

