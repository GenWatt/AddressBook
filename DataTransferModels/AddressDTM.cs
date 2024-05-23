using AddressBook.Models;

namespace AddressBook.DataTransferModels;

public class AddressDTM
{
    public IEnumerable<AddressModel> UsersSuggestions { get; set; } = new List<AddressModel>();
    public FilterDTM Filter { get; set; } = new();
    public int TotalCount { get; set; }
}
