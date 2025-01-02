namespace RabbitMQ.Shared.Contracts
{
    public interface IPublisher
    {
        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
        Task PublishAsync(object notification, CancellationToken cancellationToken = default);
    }
}