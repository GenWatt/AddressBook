using AddressBook.DataTransferModels;
using AddressBook.Models;

namespace AddressBook.Repository.AddressRepository;

public interface IAddressRepository : IRepository<AddressModel>
{
    Task<AddressModel?> GetByCity(string city);
    Task<IEnumerable<AddressModel>> GetAllWithUser(FilterDTM filter);

    Task<int> Count(FilterDTM filter);
}

