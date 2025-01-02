using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Shared.Contracts;
using RabbitMQ.Shared.Contracts.Handler;
using RabbitMQ.Shared.Handler;

namespace RabbitMQ.Shared.Wrappers
{
    public abstract class NotificationHandlerWrapperBase
    {
        public abstract Task Handle(INotification notification, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    public class NotificationHandlerWrapper<TNotification> : NotificationHandlerWrapperBase
        where TNotification : INotification
    {
        public override async Task Handle(INotification notification, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var handlers = (IEnumerable<INotificationHandler<TNotification>>) serviceProvider.GetServices(typeof(IEnumerable<INotificationHandler<TNotification>>));

            if (handlers == null)
                return;

            var tasks = handlers.Select(handler => handler.Handle((TNotification)notification, cancellationToken));
            await Task.WhenAll(tasks);
        }
    }
}