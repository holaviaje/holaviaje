using HolaViaje.Catalog.Features.Experiences.Repository;

namespace HolaViaje.Catalog.Features.Experiences;

public static class ServiceRegistration
{
    public static IServiceCollection AddExperiencesFeature(this IServiceCollection services)
    {
        services.AddScoped<IExperienceRepository, ExperienceRepository>();
        services.AddScoped<IExperienceApplication, ExperienceApplication>();
        return services;
    }
}
