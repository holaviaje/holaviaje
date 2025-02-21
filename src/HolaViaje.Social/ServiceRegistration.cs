using HolaViaje.Social.Features;

namespace HolaViaje.Social;

public static class ServiceRegistration
{
    public static void AddSocialServices(this IServiceCollection services)
    {
        services.AddAllFeatures();
    }
}