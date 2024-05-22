using AddressBook.Data;
using AddressBook.Models;
using AddressBook.Repository.AddressRepository;
using AddressBook.Services.AddressService;
using AddressBook.UOW;
using Microsoft.EntityFrameworkCore;

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

    public static void SeedData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var services = serviceScope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();

        if (!context.Addresses.Any())
        {
            var address1 = new AddressModel
            {
                City = "New York",
                Street = "5th Avenue",
                Zip = "10001",
                Country = "USA",
                CreatedAt = DateTime.Now
            };

            var address2 = new AddressModel
            {
                City = "Los Angeles",
                Street = "Hollywood Boulevard",
                Zip = "90028",
                Country = "USA",
                CreatedAt = DateTime.Now.AddDays(-1)
            };

            var address3 = new AddressModel
            {
                City = "Chicago",
                Street = "Michigan Avenue",
                Zip = "60601",
                Country = "USA",
                CreatedAt = DateTime.Now.AddDays(-2),
                UpdatedAt = DateTime.Now.AddMonths(-1)
            };

            context.Addresses.AddRange(address1, address2, address3);
            context.SaveChanges();
        }
    }   
}

