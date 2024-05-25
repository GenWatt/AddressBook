using System.Reflection;
using AddressBook.Common;
using AddressBook.Data;
using AddressBook.Models;
using AddressBook.Repository.AddressRepository;
using AddressBook.Repository.UserRepository;
using AddressBook.Services.FileService;
using AddressBook.Services.UserService;
using AddressBook.UOW;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AddressBook.Helpers;

public static class Helpers
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileService, FileService>();

        // Automapper
        services.AddAutoMapper(typeof(CustomProfile));

        // validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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
                FirstName = "user1",
                Surname = "user11",
                UserName = "user1@op.pl",
                Email = "user1@op.pl",
            };

            var user2 = new UserModel
            {
                FirstName = "user2",
                Surname = "user22",
                UserName = "user2@op.pl",
                Email = "user2@op.pl",
            };

            var user3 = new UserModel
            {
                FirstName = "user3",
                Surname = "user33",
                UserName = "user3@op.pl",
                Email = "user3@op.pl",
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

            var user4 = new UserModel
            {
                FirstName = "user4",
                Surname = "user44",
                UserName = "user4@op.pl",
                Email = "user4@op.pl",
                EmailConfirmed = true,
            };

            var user5 = new UserModel
            {
                FirstName = "user5",
                Surname = "user55",
                UserName = "user5@op.pl",
                Email = "user5@op.pl",
                EmailConfirmed = true,
            };

            var address4 = new AddressModel
            {
                City = "San Francisco",
                Street = "Market Street",
                Zip = "94103",
                Country = "USA",
                CountryCode = "US",
                CountryFlagUrl = "https://cdn.jsdelivr.net/npm/country-flag-emoji-json@2.0.0/dist/images/US.svg",
                CreatedAt = DateTime.Now.AddDays(-3),
                User = user4
            };

            var address5 = new AddressModel
            {
                City = "Boston",
                Street = "Boylston Street",
                Zip = "02116",
                Country = "USA",
                CountryCode = "US",
                CountryFlagUrl = "https://cdn.jsdelivr.net/npm/country-flag-emoji-json@2.0.0/dist/images/US.svg",
                CreatedAt = DateTime.Now.AddDays(-4),
                User = user5
            };

            Console.WriteLine("Seeding data...");
            user1.Address = address1;
            user2.Address = address2;
            user3.Address = address3;
            user4.Address = address4;
            user5.Address = address5;

            await userManager.CreateAsync(user1, "Password123!");
            await userManager.CreateAsync(user2, "Password123!");
            await userManager.CreateAsync(user3, "Password123!");
            await userManager.CreateAsync(user4, "Password123!");
            await userManager.CreateAsync(user5, "Password123!");

            await context.SaveChangesAsync();
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

