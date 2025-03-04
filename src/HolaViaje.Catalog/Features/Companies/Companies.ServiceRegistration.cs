using HolaViaje.Catalog.Features.Companies.Repository;

namespace HolaViaje.Catalog.Features.Companies;

public static class ServiceRegistration
{
    public static IServiceCollection AddCompaniesFeature(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyApplication, CompanyApplication>();
        return services;
    }
}
