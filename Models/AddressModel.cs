using Microsoft.AspNetCore.Identity;

namespace AddressBook.Models;

public class AddressModel : BaseEntity
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string Zip { get; set; }
    public string CountryFlagUrl { get; set; }

    public UserModel User { get; set; }
}

