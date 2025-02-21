using HolaViaje.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace HolaViaje.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddMessaging();
    }
}
