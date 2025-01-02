using RabbitMQ.Shared.Contracts;
using RabbitMQ.Shared.Contracts.Handler;
using RabbitMQ.Shared.Messages;
using RabbitMQ.Shared.Responses;

namespace RabbitMQ.Shared.Handler
{
    public class MediatorHandler(IMediator mediator) : IMediatorHandler
    {
        private readonly IMediator _mediator = mediator;

        public Task PublishEvent<T>(T events) where T : Event => _mediator.PublishAsync(events);
        
        public Task<Response<T>> SendCommand<T>(T command) where T : Command<T> => _mediator.SendAsync(command);
    }
}
