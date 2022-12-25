using Mediator;

namespace FediNet.Infrastructure;

public abstract class SyncRequestHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest<Unit>
{
    public abstract void Handle(TRequest request, CancellationToken cancellationToken);
    ValueTask<Unit> IRequestHandler<TRequest, Unit>.Handle(TRequest request, CancellationToken cancellationToken)
    {
        Handle(request, cancellationToken);
        return Unit.ValueTask;
    }
}

public abstract class SyncRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public abstract TResponse Handle(TRequest request, CancellationToken cancellationToken);
    ValueTask<TResponse> IRequestHandler<TRequest, TResponse>.Handle(TRequest request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(Handle(request, cancellationToken));
    }
}
