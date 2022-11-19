using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace FediNet.Features.WellKnown;

public partial class WellKnownController : MediatrControllerBase
{
    public WellKnownController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("/.well-known/nodeinfo")]
    public Task<IActionResult> WellKnownNodeInfo() => SendWithResultAsJson(new NodeInfo.Query());
}
