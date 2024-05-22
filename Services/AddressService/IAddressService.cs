using AddressBook.Models;

namespace AddressBook.Services.AddressService;

public interface IAddressService
{
    Task<AddressModel?> GetById(int id);
    Task<AddressModel?> GetById(AddressModel address);
    Task<AddressModel?> GetByCity(string city);
    Task<IEnumerable<AddressModel>> GetAll();

    Task Add(AddressModel address);
    Task Update(AddressModel address);

    Task Delete(int id);
    Task Delete(AddressModel address);
}

