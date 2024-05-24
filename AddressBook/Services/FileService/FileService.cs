using AddressBook.DataTransferModels;
using Newtonsoft.Json;

namespace AddressBook.Services.FileService;

public class FileService : IFileService
{
    public async Task<CountryDictionary?> GetCountryData()
    {
        var jsonFile = await File.ReadAllTextAsync("wwwroot/Json/CountryData.json");
        return JsonConvert.DeserializeObject<CountryDictionary>(jsonFile);
    }
}