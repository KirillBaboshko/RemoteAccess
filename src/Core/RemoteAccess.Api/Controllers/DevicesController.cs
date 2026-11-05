using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteAccess.Application.Features.Devices.Commands;
using RemoteAccess.Application.Features.Devices.Models;

namespace RemoteAccess.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class DevicesController: ControllerBase
    {
        private readonly ISender _sender;

        public DevicesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public Task<Guid> CreateDevice([FromBody] DeviceInfo deviceInfo, CancellationToken cancellationToken)
        {
            return _sender.Send(new CreateDeviceCommand(deviceInfo), cancellationToken);
        }
    }
}
