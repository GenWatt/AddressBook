using System.ComponentModel.DataAnnotations;

namespace AddressBook.DataTransferModels;

public class UserDataDTM
{
    public SelectDTM SelectData { get; set; } = new();
    public CountryDictionary CountryData = new();

    public string? Id { get; set; }

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    [Required]
    [Display(Name = "Phone Number Code")]
    public string PhoneNumberCode { get; set; }

    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    [DataType(DataType.PostalCode)]
    public string Zip { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string CountryCode { get; set; }
    [Required]
    public string CountryFlagUrl { get; set; }
}

public class UserDataPostDTM
{
    public string? Id { get; set; }

    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    [Required]
    [Display(Name = "Phone Number Code")]
    public string PhoneNumberCode { get; set; }

    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    [DataType(DataType.PostalCode)]
    public string Zip { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string CountryCode { get; set; }
    [Required]
    public string CountryFlagUrl { get; set; }
}