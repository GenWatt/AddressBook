using AddressBook.Models;

namespace AddressBook.Repository.AddressRepository;

public interface IAddressRepository : IRepository<AddressModel>
{
    Task<AddressModel?> GetByCity(string city);
}

