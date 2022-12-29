using Mediator;

namespace FediNet.Infrastructure;

public interface IHttpRequest : IRequest<IResult>
{
}

public interface IHttpRequestHandler<TRequest> : IRequestHandler<TRequest, IResult> where TRequest : IHttpRequest
{
}
