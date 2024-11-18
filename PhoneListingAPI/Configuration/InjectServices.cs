using System;
using PhoneListingAPI.Services;
using PhoneListingAPI.Services.ListingServices;

namespace PhoneListingAPI.Configuration;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IListingService, ListingServise>();
        return services;
    }
}
