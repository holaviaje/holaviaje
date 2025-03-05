using HolaViaje.Catalog.Features;

namespace HolaViaje.Catalog;

public static class ServiceRegistration
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services)
    {
        services.AddAllFeatures();
        return services;
    }
}
