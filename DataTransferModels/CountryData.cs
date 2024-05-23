using Newtonsoft.Json;


namespace AddressBook.DataTransferModels;

public class CountryData
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("region")]
    public string Region { get; set; }

    [JsonProperty("timezones")]
    public Dictionary<string, string> Timezones { get; set; }

    [JsonProperty("iso")]
    public IsoData Iso { get; set; }

    [JsonProperty("phone")]
    public List<string> Phone { get; set; }

    [JsonProperty("emoji")]
    public string Emoji { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }
}

public class IsoData
{
    [JsonProperty("alpha-2")]
    public string Alpha2 { get; set; }

    [JsonProperty("alpha-3")]
    public string Alpha3 { get; set; }

    [JsonProperty("numeric")]
    public string Numeric { get; set; }
}

public class CountryDictionary : Dictionary<string, CountryData>
{
}
