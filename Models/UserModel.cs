using Microsoft.AspNetCore.Identity;

namespace AddressBook.Models;

public class UserModel : IdentityUser, IBaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int AddressId { get; set; }
    public AddressModel Address { get; set; } = new();
    public string PhoneNumberCode { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {Surname}";

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    int IBaseEntity.Id { get; set; }

    public List<AddressModel> Addresses { get; set; } = new();
}