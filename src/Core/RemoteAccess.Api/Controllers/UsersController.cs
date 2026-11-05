using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteAccess.Application.Features.Devices.Commands;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Application.Features.Users.Queries;
using RemoteAccess.Data.DataProviders.Users.Model;

namespace RemoteAccess.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("profile")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public Task<UserModel> GetUserProfile([FromBody] Guid UserId, CancellationToken cancellationToken)
        {
            return _sender.Send(new UserProfileQuery(UserId), cancellationToken);
        }
    }
}
