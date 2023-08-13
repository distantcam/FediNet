using Mediator;

namespace System.Threading.Tasks;

public abstract class SyncHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    protected abstract TResponse Handle(TRequest request);

    public ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return ValueTask.FromCanceled<TResponse>(cancellationToken);
        try
        {
            return ValueTask.FromResult(Handle(request));
        }
        catch (Exception ex)
        {
            return ValueTask.FromException<TResponse>(ex);
        }
    }
}
