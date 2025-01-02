using RabbitMQ.Shared.Contracts;
using RabbitMQ.Shared.Contracts.Handler;

namespace RabbitMQ.Shared.Wrappers
{
    public abstract class RequestHandlerBase
    {
        public abstract Task<object?> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    public abstract class RequestHandler<T> 
    {
        public abstract Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
        public abstract Task Handle(T request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    public abstract class RequestHandlerWrapperBase<TResponse> : RequestHandlerBase
    {
        public abstract Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }

    public class RequestHandlerWrapper<TRequest, TResponse> : RequestHandlerWrapperBase<TResponse> where TRequest : IRequest<TResponse>
    {
        public override async Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return serviceProvider.GetService(typeof(IRequestHandler<TRequest, TResponse>)) is not IRequestHandler<TRequest, TResponse> handler
            ? throw new InvalidOperationException($"Handler for {typeof(TRequest).Name} not found.")
                : await handler.Handle((TRequest)request, cancellationToken);
        }

        public override async Task<object?> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return await Handle((IRequest<TResponse>)request, serviceProvider, cancellationToken);
        }
    }

    public class RequestHandlerWrapper<TRequest> : RequestHandler<TRequest> where TRequest : IRequest
    {
        public override async Task Handle(TRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            IRequestHandler<TRequest>? handler = (IRequestHandler<TRequest>?)serviceProvider.GetService(typeof(IRequestHandler<TRequest>)) 
            ?? throw new InvalidOperationException($"Handler for {typeof(TRequest).Name} not found.");
            await handler.Handle(request, cancellationToken);
        }

        public override async Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            await Handle((IRequest) request, serviceProvider, cancellationToken);
        }
    }





}