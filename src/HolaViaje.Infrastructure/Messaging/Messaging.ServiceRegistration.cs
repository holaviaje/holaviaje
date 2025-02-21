using Microsoft.Extensions.DependencyInjection;

namespace HolaViaje.Infrastructure.Messaging;

public static class ServiceRegistration
{
    public static void AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, EventBus>();
    }
}
