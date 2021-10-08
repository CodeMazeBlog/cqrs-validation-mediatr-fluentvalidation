using MediatR;

namespace Application.Abstractions.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}