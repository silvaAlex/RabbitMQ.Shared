using RabbitMQ.Shared.Contracts;

namespace RabbitMQ.Shared.Contracts.Handler
{
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}