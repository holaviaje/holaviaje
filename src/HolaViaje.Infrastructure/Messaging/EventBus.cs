using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HolaViaje.Infrastructure.Messaging;

public class EventBus(IServiceScopeFactory serviceScopeFactory) : IEventBus, IDisposable
{
    private readonly IServiceScope _serviceScope = serviceScopeFactory.CreateScope();

    public async Task Publish<T>(T message) where T : class
    {
        var topicProducer = _serviceScope.ServiceProvider.GetRequiredService<ITopicProducer<T>>();

        if (topicProducer == null)
        {
            throw new InvalidOperationException($"No topic producer found for {typeof(T).Name}");
        }

        await topicProducer.Produce(message);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
