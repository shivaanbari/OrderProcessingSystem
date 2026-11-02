using MassTransit;
using OrderService.Application.Abstractions;

namespace OrderService.Infrastructure.Messaging;

public class MassTransitEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken)
        where T : class
    {
        await _publishEndpoint.Publish(message, cancellationToken);
    }
}