using Core.Identity;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace VendingOnlineStore.Identity;

internal static class DependencyInjection
{
    public static IServiceCollection AddAspNetCoreIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<AppDbContext>();
        services.AddScoped<ISignInManager<User>, MicrosoftSingInManager>();
        services.AddScoped<IUserManager, MicrosoftUserManager>();
        services.AddScoped<IUserIdentityProvider, HttpContextUserIdentityProvider>();
        return services;
    }
}