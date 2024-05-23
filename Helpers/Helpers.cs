using AddressBook.Data;
using AddressBook.Models;
using AddressBook.Repository.AddressRepository;
using AddressBook.Services.AddressService;
using AddressBook.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AddressBook.Helpers;

public static class Helpers
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IAddressRepository, AddressRepository>();
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }

    public static async Task SeedData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var services = serviceScope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<UserModel>>();

        if (!context.Addresses.Any())
        {
            var user1 = new UserModel
            {
                UserName = "user1",
                Email = "user1@op.pl",
                EmailConfirmed = true,
            };

            var user2 = new UserModel
            {
                UserName = "user2",
                Email = "user2@op.pl",
                EmailConfirmed = true,
            };

            var user3 = new UserModel
            {
                UserName = "user3",
                Email = "user3@op.pl",
                EmailConfirmed = true,
            };

            var address1 = new AddressModel
            {
                City = "New York",
                Street = "5th Avenue",
                Zip = "10001",
                Country = "USA",
                CountryCode = "US",
                CountryFlagUrl = "https://cdn.jsdelivr.net/npm/country-flag-emoji-json@2.0.0/dist/images/US.svg",
                CreatedAt = DateTime.Now,
                User = user1
            };

            var address2 = new AddressModel
            {
                City = "Los Angeles",
                Street = "Hollywood Boulevard",
                Zip = "90028",
                Country = "USA",
                CountryCode = "US",
                CountryFlagUrl = "https://cdn.jsdelivr.net/npm/country-flag-emoji-json@2.0.0/dist/images/US.svg",
                CreatedAt = DateTime.Now.AddDays(-1),
                User = user2
            };

            var address3 = new AddressModel
            {
                City = "Chicago",
                Street = "Michigan Avenue",
                Zip = "60601",
                Country = "USA",
                CountryCode = "US",
                CountryFlagUrl = "https://cdn.jsdelivr.net/npm/country-flag-emoji-json@2.0.0/dist/images/US.svg",
                CreatedAt = DateTime.Now.AddDays(-2),
                UpdatedAt = DateTime.Now.AddMonths(-1),
                User = user3
            };
            user1.Address = address1;
            user2.Address = address2;
            user3.Address = address3;
            await userManager.CreateAsync(user1, "Password123!");
            await userManager.CreateAsync(user2, "Password123!");
            await userManager.CreateAsync(user3, "Password123!");
        }
    }

    public static string IsActive(this IHtmlHelper html, string controller, string action)
    {
        var routeData = html.ViewContext.RouteData.Values;
        var routeAction = routeData["Action"].ToString();
        var routeController = routeData["Controller"].ToString();

        var returnActive = controller == routeController && action == routeAction;

        return returnActive ? "show" : "";
    }
}

