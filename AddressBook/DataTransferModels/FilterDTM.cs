namespace AddressBook.DataTransferModels;

public enum FilterBy
{
    FirstName,
    Surname,
    Email,
    Phone,
    Address,
    City,
    State,
    Zip,
    Country
}

public class FilterDTM
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 4;
    public FilterBy FilterBy { get; set; } = FilterBy.FirstName;
    public string UserId { get; set; } = string.Empty;
}