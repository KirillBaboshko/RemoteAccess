using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteAccess.Common.Extensions;
using RemoteAccess.Application.Features.Devices.Commands;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Application.Features.Devices.Queries;
using RemoteAccess.Data.DataProviders.Devices.Model;
using RemoteAccess.Domain.Models.Devices;

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

        [HttpGet]
        [ProducesResponseType(typeof(List<DeviceModel>), StatusCodes.Status200OK)]
        public Task<List<DeviceModel>> GetUserDevices(CancellationToken cancellationToken)
        {
            return _sender.Send(new UserDevicesQuery(User.GetUserId()), cancellationToken);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(List<DeviceModel>), StatusCodes.Status200OK)]
        public Task<List<DeviceModel>> SearchDevices([FromQuery] string? search, CancellationToken cancellationToken)
        {
            return _sender.Send(new SearchDevicesQuery(search ?? string.Empty), cancellationToken);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(DeviceRegistrationResult), StatusCodes.Status200OK)]
        public Task<DeviceRegistrationResult> RegisterDevice([FromBody] DeviceInfo deviceInfo, CancellationToken cancellationToken)
        {
            return _sender.Send(new AppendDeviceCommand(deviceInfo, User.GetUserId()), cancellationToken);
        }

        [HttpGet("{deviceId:guid}/access")]
        [ProducesResponseType(typeof(DeviceAccessModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeviceAccessModel>> GetAccess(Guid deviceId, CancellationToken cancellationToken)
        {
            var access = await _sender.Send(new DeviceAccessQuery(deviceId, User.GetUserId()), cancellationToken);
            return access is null ? NotFound() : Ok(access);
        }

        [HttpPost("toggle-status")]
        [ProducesResponseType(typeof(DeviceStatus), StatusCodes.Status200OK)]
        public Task<DeviceStatus> ToggleStatus([FromBody] DeviceStatusRequest request, CancellationToken cancellationToken)
        {
            return _sender.Send(new ToggleDeviceStatusCommand(request, User.GetUserId()), cancellationToken);
        }

    }

}
