using RabbitMQ.Shared.Contracts;
using RabbitMQ.Shared.Wrappers;

namespace RabbitMQ.Shared
{
    public class Mediator(IServiceProvider serviceProvider) : IMediator
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            var notificationType = notification.GetType();
            var wrapperType = typeof(NotificationHandlerWrapper<>).MakeGenericType(notificationType);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper for type {notificationType}");

            var handler = (NotificationHandlerWrapper<INotification>)wrapper;

            await handler.Handle(notification, _serviceProvider, cancellationToken);
        }

        public async Task PublishAsync(object notification, CancellationToken cancellationToken = default)
        {
            var notificationType = notification.GetType();
            var wrapperType = typeof(NotificationHandlerWrapper<>).MakeGenericType(notificationType);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper for type {notificationType}");

            var handler = (NotificationHandlerWrapper<INotification>)wrapper;

            await handler.Handle((INotification)notification, _serviceProvider, cancellationToken);
        }

        public Task SendAsync(IRequest request, CancellationToken cancellationToken = default) 
        {
            var notificationType = request.GetType();
            var wrapperType = typeof(RequestHandlerWrapper<IRequest>).MakeGenericType(notificationType);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper for type {notificationType}");
            var handler = (RequestHandlerWrapper<IRequest>)wrapper ?? throw new InvalidOperationException($"Could not create handle for type {notificationType}");
            return handler.Handle(request, _serviceProvider, cancellationToken);
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var notificationType = request.GetType();
            var wrapperType = typeof(RequestHandlerWrapper<IRequest<TResponse>, TResponse>).MakeGenericType(notificationType);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper for type {notificationType}");

            var handler = (RequestHandlerWrapper<IRequest<TResponse>, TResponse>)wrapper;

            return await handler.Handle(request, _serviceProvider, cancellationToken);
        }
    }
}
