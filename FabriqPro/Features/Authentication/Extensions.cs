using FabriqPro.Features.Authentication.Repositories;
using FabriqPro.Features.Authentication.Services;

namespace FabriqPro.Features.Authentication;

public static class Extensions
{
    public static void RegisterAuthenticationFeature(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<AuthRepository>();
    }
}