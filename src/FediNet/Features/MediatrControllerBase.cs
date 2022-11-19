using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace FediNet.Features;

[ApiController]
public abstract class MediatrControllerBase : ControllerBase
{
    private readonly IMediator _mediator;
    public MediatrControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected Task<IActionResult> SendOk<TRequest>(TRequest request) where TRequest : notnull
    {
        return Send(request, async () =>
        {
            await _mediator.Send(request);
            return Ok();
        });
    }

    protected Task<IActionResult> SendCreated<TRequest>(TRequest request) where TRequest : notnull
    {
        return Send(request, async () =>
        {
            var response = await _mediator.Send(request);
            return StatusCode(201, response);
        });
    }

    protected Task<IActionResult> SendNoContent<TRequest>(TRequest request) where TRequest : notnull
    {
        return Send(request, async () =>
        {
            await _mediator.Send(request);
            return NoContent();
        });
    }

    protected Task<IActionResult> SendWithResultAsJson<TRequest>(TRequest request) where TRequest : notnull
    {
        return Send(request, async () =>
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        });
    }

    protected Task<IActionResult> SendWithStatus<TRequest>(TRequest request, int statusCode) where TRequest : notnull
    {
        return Send(request, async () =>
        {
            var response = await _mediator.Send(request);
            return StatusCode(statusCode, response);
        });
    }

    private async Task<IActionResult> Send<TRequest>(TRequest request, Func<Task<IActionResult>> func)
    {
        return await func();
    }
}
