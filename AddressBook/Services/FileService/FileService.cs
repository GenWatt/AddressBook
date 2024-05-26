using AddressBook.Common;
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

    public PostalCodeDataRules GetPostalRulesByCountryCode(string countryCode)
    {
        var jsonFile = File.ReadAllText("wwwroot/Json/PostalCodeRules.json");
        var rule = JsonConvert.DeserializeObject<List<PostalCodeDataRules>>(jsonFile).FirstOrDefault(x => x.ISO == countryCode);

        return rule;
    }
}