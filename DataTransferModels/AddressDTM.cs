using AddressBook.Models;

namespace AddressBook.DataTransferModels;

public class AddressDTM
{
    public IEnumerable<UserModel> UsersSuggestions { get; set; } = new List<UserModel>();
    public FilterDTM Filter { get; set; } = new();
    public int TotalCount { get; set; }
}
