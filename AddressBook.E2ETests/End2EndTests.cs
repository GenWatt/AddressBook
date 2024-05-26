using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AddressBook.E2ETests;

public class End2EndTests : End2EndBase, IDisposable
{
    private const string BaseUrl = "http://localhost:5188/";
    private const string RegisterUrl = BaseUrl + "Identity/Account/Register";
    private const string EmailId = "Input_Email";
    private const string PasswordId = "Input_Password";
    private const string ConfirmPasswordId = "Input_ConfirmPassword";
    private const string FirstNameId = "Input_FirstName";
    private const string SurnameId = "Input_Surname";
    private const string StreetId = "Input_Street";
    private const string CityId = "Input_City";
    private const string ZipCodeId = "Input_Zip";
    private const string PhoneNumberId = "Input_PhoneNumber";
    private const string RegisterSubmitId = "registerSubmit";
    private const string SurnameErrorId = "Input_Surname-error";
    private const string ZipCodeErrorId = "Input_Zip-error";
    private const string SurnameErrorMessage = "The Surname field is required.";
    private const string ZipCodeErrorMessage = "Invalid postal code for United States.";
    private const string LoginId = "login-submit";
    private const string UserEmailId = "currentUserEmail";
    private const string CountrySelectId = "countrySelect";
    private const string PhoneCodeSelectId = "PhoneCodeSelect";

    public End2EndTests() : base() { }
    private void Login()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseUrl);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var emailInput = driver.FindElement(By.Id(EmailId));
        var passwordInput = driver.FindElement(By.Id(PasswordId));
        var loginButton = driver.FindElement(By.Id(LoginId));

        // Act
        emailInput.SendKeys("user1@op.pl");
        passwordInput.SendKeys("Password123!");
        MakeScreenshot("login");
        loginButton.Click();
        // Assert
        Assert.Contains(BaseUrl, driver.Url);
        // check if there is strong element with user email
        var userEmail = wait.Until(d => driver.FindElement(By.Id(UserEmailId)));
        MakeScreenshot("email");
        Assert.Equal("user1@op.pl!", userEmail.Text);
    }

    private void RegisterArrangeAndAct(string firstNameValue, string surnameValue, string emailValue, string streetValue, string cityValue, string zipCodeValue, string phoneCodeValue, string phoneNumberValue, string countryCodeValue, string passwordValue, string confirmPasswordValue)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        // Arrange
        var firstName = wait.Until(d => d.FindElement(By.Id(FirstNameId)));
        var surname = wait.Until(d => d.FindElement(By.Id(SurnameId)));
        var street = wait.Until(d => d.FindElement(By.Id(StreetId)));
        var city = wait.Until(d => d.FindElement(By.Id(CityId)));
        var zipCode = wait.Until(d => d.FindElement(By.Id(ZipCodeId)));
        var countryCode = wait.Until(d => d.FindElement(By.Id(CountrySelectId)));
        var phoneNumber = wait.Until(d => d.FindElement(By.Id(PhoneNumberId)));
        var phoneNumberCountryCode = wait.Until(d => d.FindElement(By.Id(PhoneCodeSelectId)));
        var email = wait.Until(d => d.FindElement(By.Id(EmailId)));
        var password = wait.Until(d => d.FindElement(By.Id(PasswordId)));
        var confirmPassword = wait.Until(d => d.FindElement(By.Id(ConfirmPasswordId)));
        var registerButton = wait.Until(d => d.FindElement(By.Id(RegisterSubmitId)));
        var phoneCodeSelect = new SelectElement(phoneNumberCountryCode);
        var countryCodeSelect = new SelectElement(countryCode);

        // Act
        firstName.SendKeys(firstNameValue);
        surname.SendKeys(surnameValue);
        street.SendKeys(streetValue);
        city.SendKeys(cityValue);
        zipCode.SendKeys(zipCodeValue);
        phoneCodeSelect.SelectByValue(phoneCodeValue);
        phoneNumber.SendKeys(phoneNumberValue);
        countryCodeSelect.SelectByValue(countryCodeValue);
        email.SendKeys(emailValue);
        password.SendKeys(passwordValue);
        confirmPassword.SendKeys(confirmPasswordValue);
        MakeScreenshot("register");
        registerButton.Click();
    }

    [Theory]
    [InlineData("John", "Doe", "John@op.pl", "Main Street", "New York", "12345", "US +1", "123456789", "US", "123", "123")]
    [InlineData("Jane", "Smith", "Jane@op.pl", "Second Street", "Los Angeles", "67890", "US +1", "987654321", "US", "456", "456")]
    public void Register_ShouldNavigateToHomePageAndHaveUserEmail(string firstNameValue, string surnameValue, string emailValue, string streetValue, string cityValue, string zipCodeValue, string phoneCodeValue, string phoneNumberValue, string countryCodeValue, string passwordValue, string confirmPasswordValue)
    {
        Console.WriteLine($"{RegisterUrl}");
        driver.Navigate().GoToUrl($"{RegisterUrl}");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        RegisterArrangeAndAct(firstNameValue, surnameValue, emailValue, streetValue, cityValue, zipCodeValue, phoneCodeValue, phoneNumberValue, countryCodeValue, passwordValue, confirmPasswordValue);
        // Assert
        Assert.Contains(BaseUrl, driver.Url);
        // check if there is strong element with user email
        var userEmail = wait.Until(d => d.FindElement(By.Id(UserEmailId)));
        MakeScreenshot("email-register");
        Assert.Equal($"{emailValue}!", userEmail.Text);
    }

    [Theory]
    [InlineData("John", "", "John@op.pl", "Main Street", "New York", "12345", "US +1", "123456789", "US", "123", "123")]
    [InlineData("Jane", "", "Jane@op.pl", "Second Street", "Los Angeles", "67890", "US +1", "987654321", "US", "456", "456")]
    public void Register_ShouldShowValidationError(string firstNameValue, string surnameValue, string emailValue, string streetValue, string cityValue, string zipCodeValue, string phoneCodeValue, string phoneNumberValue, string countryCodeValue, string passwordValue, string confirmPasswordValue)
    {
        driver.Navigate().GoToUrl($"{RegisterUrl}");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        RegisterArrangeAndAct(firstNameValue, surnameValue, emailValue, streetValue, cityValue, zipCodeValue, phoneCodeValue, phoneNumberValue, countryCodeValue, passwordValue, confirmPasswordValue);
        // Assert
        Assert.Equal($"{RegisterUrl}", driver.Url);
        // check if there is strong element with user email
        var surnameErrorLabel = wait.Until(d => d.FindElement(By.Id(SurnameErrorId)));
        MakeScreenshot("email-register-error");
        Assert.Equal(SurnameErrorMessage, surnameErrorLabel.Text);
    }

    [Theory]
    [InlineData("John", "LOl", "John@op.pl", "Main Street", "New York", "12", "US +1", "123456789", "US", "123", "123")]
    [InlineData("Jane", "LOl", "Jane@op.pl", "Second Street", "Los Angeles", "677", "US +1", "987654321", "US", "456", "456")]
    public void Register_ShouldShowZipCodeBeInvalid(string firstNameValue, string surnameValue, string emailValue, string streetValue, string cityValue, string zipCodeValue, string phoneCodeValue, string phoneNumberValue, string countryCodeValue, string passwordValue, string confirmPasswordValue)
    {
        driver.Navigate().GoToUrl($"{RegisterUrl}");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        RegisterArrangeAndAct(firstNameValue, surnameValue, emailValue, streetValue, cityValue, zipCodeValue, phoneCodeValue, phoneNumberValue, countryCodeValue, passwordValue, confirmPasswordValue);
        // Assert
        Assert.Equal($"{RegisterUrl}", driver.Url);
        // check if there is strong element with user email
        var surnameErrorLabel = wait.Until(d => d.FindElement(By.CssSelector(".field-validation-error")));
        MakeScreenshot("email-register-error-zip");
        Assert.Equal(ZipCodeErrorMessage, surnameErrorLabel.Text);
    }

    [Fact]
    public void Login_ShouldNavigateToHomePageAndAddAddress()
    {
        Login();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        // find a tag with text "Add Address"
        var addAddressButton = wait.Until(d => d.FindElement(By.LinkText("Add Address")));

        addAddressButton.Click();

        var addButton = wait.Until(d => d.FindElement(By.LinkText("Add")));
        addButton.Click();

        var tableTr = wait.Until(d => d.FindElement(By.XPath("//table/tbody/tr")));
        var firstTd = tableTr.FindElement(By.XPath("./td[1]"));

        Assert.Contains("@", firstTd.Text);
        Assert.Equal(BaseUrl, driver.Url);
    }

    [Fact]
    public void Login_ShouldNavigateToHomePageAndHaveUserEmail()
    {
        Login();
    }
}