namespace OrderService.Application.Abstractions;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken)
        where T : class;
}