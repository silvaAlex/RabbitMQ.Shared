using RabbitMQ.Shared.Messages;
using RabbitMQ.Shared.Responses;

namespace RabbitMQ.Shared.Contracts.Handler
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T events) where T : Event;
        Task<Response<T>> SendCommand<T>(T command) where T : Command<T>;
    }
}
