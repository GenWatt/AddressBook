// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AddressBook.Common;
using AddressBook.DataTransferModels;
using AddressBook.Models;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AddressBook.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IUserStore<UserModel> _userStore;
        private readonly IUserEmailStore<UserModel> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly string countryDataPath = "wwwroot/Json/CountryData.json";
        private readonly string postalCodeDataPath = "wwwroot/Json/PostalCodeRules.json";

        public RegisterModel(
            UserManager<UserModel> userManager,
            IUserStore<UserModel> userStore,
            SignInManager<UserModel> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            Input = new InputModel();
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public UserDataDTM UserData { get; set; } = new();
        public List<PostalCodeDataRules> PostalCodeDataRules { get; set; } = new();
        public bool FirstLoad { get; set; } = true;
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

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

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            await GetCountryData();
        }

        private async Task GetCountryData()
        {
            var jsonFile = await System.IO.File.ReadAllTextAsync(countryDataPath);
            var countryData = JsonConvert.DeserializeObject<CountryDictionary>(jsonFile);
            UserData.CountryData = countryData;
            UserData.SelectData.CountryData = countryData;
        }

        private async Task GetPostalCodeData()
        {
            var jsonFile = await System.IO.File.ReadAllTextAsync(postalCodeDataPath);
            var postalCodeData = JsonConvert.DeserializeObject<List<PostalCodeDataRules>>(jsonFile);
            PostalCodeDataRules = postalCodeData;
        }

        private bool IsValidPostalCode(string postalCode, string countryCode)
        {
            var postalCodeData = PostalCodeDataRules.FirstOrDefault(x => x.ISO == countryCode);

            if (postalCodeData == null)
            {
                return false;
            }

            var regex = new Regex(postalCodeData.Regex);
            return regex.IsMatch(postalCode);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/Identity/Account/Logout")
            {
                returnUrl = Url.Content("~/");
            }

            await GetPostalCodeData();

            if (!IsValidPostalCode(Input.Zip, Input.CountryCode))
            {
                ModelState.AddModelError("Input.Zip", $"Invalid postal code for {Input.Country}.");
            }

            if (ModelState.IsValid)
            {
                var user = new UserModel
                {
                    FirstName = Input.FirstName,
                    Surname = Input.Surname,
                    UserName = Input.Email,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    PhoneNumberCode = Input.PhoneNumberCode,
                    Address = new AddressModel
                    {
                        Street = Input.Street,
                        City = Input.City,
                        Zip = Input.Zip,
                        Country = Input.Country,
                        CountryCode = Input.CountryCode,
                        CountryFlagUrl = Input.CountryFlagUrl
                    }
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("Login", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            await GetCountryData();

            FirstLoad = false;
            return Page();
        }

        private UserModel CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserModel)}'. " +
                    $"Ensure that '{nameof(UserModel)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<UserModel> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserModel>)_userStore;
        }
    }
}
