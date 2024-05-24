using AddressBook.DataTransferModels;

namespace AddressBook.Services.FileService;

public interface IFileService
{
    Task<CountryDictionary?> GetCountryData();
}