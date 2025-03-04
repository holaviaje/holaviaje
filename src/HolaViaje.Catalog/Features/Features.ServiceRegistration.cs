using HolaViaje.Catalog.Features.Companies;
using HolaViaje.Catalog.Features.Experiences;

namespace HolaViaje.Catalog.Features;

public static class ServiceRegistration
{
    public static IServiceCollection AddAllFeatures(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceRegistration).Assembly);
        services.AddCompaniesFeature();
        services.AddExperiencesFeature();
        return services;
    }
}
