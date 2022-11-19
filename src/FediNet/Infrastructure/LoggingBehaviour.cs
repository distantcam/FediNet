using MediatR;

namespace FediNet.Infrastructure;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(nameof(FediNet) + ".Pipeline");
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation("Handling request {requestType}", typeof(TRequest));
        _logger.LogDebug("Request {@request}", request);
        try
        {
            var response = await next().ConfigureAwait(false);
            _logger.LogInformation("Handled response {responseType}", typeof(TResponse));
            _logger.LogDebug("Response {@response}", response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Pipeline handler exception for request {requestType} {@request}", typeof(TRequest), request);
            throw;
        }
    }
}
