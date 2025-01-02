using RabbitMQ.Shared.Contracts;
using RabbitMQ.Shared.Responses;

namespace RabbitMQ.Shared.Messages
{
    public abstract class Command<T> : Message, IRequest<Response<T>>
    {
        public DateTime Timestamp { get; private set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}