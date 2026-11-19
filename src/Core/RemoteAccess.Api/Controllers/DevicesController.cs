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

        [HttpPost("register")]
        [ProducesResponseType(typeof(DeviceRegistrationResult), StatusCodes.Status200OK)]
        public Task<DeviceRegistrationResult> RegisterDevice([FromBody] DeviceInfo deviceInfo, CancellationToken cancellationToken)
        {
            return _sender.Send(new AppendDeviceCommand(deviceInfo, GetUserId()), cancellationToken);
        }

        [HttpPost("online")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SetOnline([FromBody] DeviceOnlineRequest request, CancellationToken cancellationToken)
        {
            await _sender.Send(new SetDeviceOnlineCommand(request, GetUserId()), cancellationToken);
            return NoContent();
        }

        private Guid GetUserId() =>
            Guid.TryParse(User.FindFirst("userId")?.Value, out var id)
                ? id
                : throw new InvalidOperationException("User ID is missing from the token.");
    }

}
