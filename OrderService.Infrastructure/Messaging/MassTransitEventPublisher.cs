using MassTransit;
using OrderService.Application.Abstractions;

namespace OrderService.Infrastructure.Messaging;

public class MassTransitEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
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