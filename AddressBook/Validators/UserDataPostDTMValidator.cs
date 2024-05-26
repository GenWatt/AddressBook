using System.Text.RegularExpressions;
using AddressBook.DataTransferModels;
using AddressBook.Services.FileService;
using FluentValidation;

namespace AddressBook.Validators;

public class UserDataPostDTMValidator : AbstractValidator<UserDataPostDTM>
{
    private readonly IFileService _fileService;

    public UserDataPostDTMValidator(IFileService fileService)
    {
        _fileService = fileService;

        RuleFor(x => x.FirstName)
            .NotEmpty();
        RuleFor(x => x.Surname)
            .NotEmpty();
        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
        RuleFor(x => x.PhoneNumberCode)
            .NotEmpty();
        RuleFor(x => x.Street)
            .NotEmpty();
        RuleFor(x => x.City)
            .NotEmpty();
        RuleFor(x => x.Zip)
            .NotEmpty()
            .Must((user, zip) => IsValidPostalCode(user.Zip, user.CountryCode))
            .WithMessage("Invalid postal code for the selected country.");
        RuleFor(x => x.Country)
            .NotEmpty();
        RuleFor(x => x.CountryCode)
            .NotEmpty();
        RuleFor(x => x.CountryFlagUrl)
            .NotEmpty();
    }

    private bool IsValidPostalCode(string postalCode, string countryCode)
    {
        var country = _fileService.GetPostalRulesByCountryCode(countryCode);
        if (country == null) return false;

        var regex = new Regex(country.Regex);
        return regex.IsMatch(postalCode);
    }
}