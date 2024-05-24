namespace AddressBook.DataTransferModels;

public class SelectDTM
{
    public string Value { get; set; }
    public CountryDictionary CountryData { get; set; } = new();
}