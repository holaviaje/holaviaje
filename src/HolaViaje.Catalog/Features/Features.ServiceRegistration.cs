using HolaViaje.Catalog.Features.Experiences;

namespace HolaViaje.Catalog.Features;

public static class ServiceRegistration
{
    public static IServiceCollection AddCatalogFeatures(this IServiceCollection services)
    {
        services.AddExperiencesFeature();
        return services;
    }
}
