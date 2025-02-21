using Microsoft.Extensions.DependencyInjection;

namespace HolaViaje.Infrastructure.BlobStorage;

public static class ServiceRegistration
{
    public static void AddBlobStorage(this IServiceCollection services)
    {
        services.AddSingleton<IBlobRepository, BlobRepository>();
    }
}
