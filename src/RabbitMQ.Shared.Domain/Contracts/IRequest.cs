namespace RabbitMQ.Shared.Contracts
{
    public interface IRequest : IRequestBase { }

    public interface  IRequest<out T> : IRequestBase { }

    public interface IRequestBase { }
}