using Mediator;

namespace FediNet.Infrastructure;

public abstract class Feature
{
    public interface IFeatureRequest : IRequest<IResult> { }

    public interface IFeatureHandler<TRequest> : IRequestHandler<TRequest, IResult>
        where TRequest : IFeatureRequest
    { }

    public abstract class SyncFeatureHandler<TRequest> : IFeatureHandler<TRequest>
        where TRequest : IFeatureRequest
    {
        protected abstract IResult Handle(TRequest request, CancellationToken cancellationToken);
        ValueTask<IResult> IRequestHandler<TRequest, IResult>.Handle(TRequest request, CancellationToken cancellationToken) =>
            ValueTask.FromResult(Handle(request, cancellationToken));
    }
}
