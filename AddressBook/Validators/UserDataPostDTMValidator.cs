using AddressBook.DataTransferModels;
using FluentValidation;

namespace AddressBook.Validators;

public class UserDataPostDTMValidator : AbstractValidator<UserDataPostDTM>
{
    public UserDataPostDTMValidator()
    {
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
            .NotEmpty();
        RuleFor(x => x.Country)
            .NotEmpty();
        RuleFor(x => x.CountryCode)
            .NotEmpty();
        RuleFor(x => x.CountryFlagUrl)
            .NotEmpty();
    }
}