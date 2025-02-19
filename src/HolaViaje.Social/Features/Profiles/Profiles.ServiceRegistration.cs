using HolaViaje.Social.Features.Profiles.Repository;

namespace HolaViaje.Social.Features.Profiles;

public static class ServiceRegistration
{
    public static IServiceCollection AddProfilesFeature(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileApplication, UserProfileApplication>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        return services;
    }
}
