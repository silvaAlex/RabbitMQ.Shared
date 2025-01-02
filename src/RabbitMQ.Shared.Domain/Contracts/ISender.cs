namespace RabbitMQ.Shared.Contracts
{
    public interface ISender
    {
        Task<object?> Send(object request, CancellationToken cancellationToken = default);
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}