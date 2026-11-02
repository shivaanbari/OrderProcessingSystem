namespace OrderService.Application.Abstractions;

public interface IEventBus
{
    Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken)
        where T : class;
}