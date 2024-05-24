namespace AddressBook.DataTransferModels;

public class PaginationDTM
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public FilterDTM? Filters { get; set; }
    public string FilterPrefix { get; set; } = string.Empty;
}
