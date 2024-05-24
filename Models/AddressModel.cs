namespace AddressBook.Models;

public class AddressModel : IBaseEntity
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
    public string Zip { get; set; }
    public string CountryFlagUrl { get; set; }

    public int UserId { get; set; }
    public UserModel User { get; set; }
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

