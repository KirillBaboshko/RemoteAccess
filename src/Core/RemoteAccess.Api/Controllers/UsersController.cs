using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteAccess.Common.Extensions;
using RemoteAccess.Application.Features.Users.Queries;
using RemoteAccess.Common.Data;
using RemoteAccess.Data.DataProviders.Sessions.Model;
using RemoteAccess.Data.DataProviders.Users.Model;
using RemoteAccess.Domain.Models.Ecxeptions;

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
        public Task<UserModel> GetUserProfile(CancellationToken cancellationToken)
        {
            return _sender.Send(new UserProfileQuery(User.GetUserId()), cancellationToken);
        }

        [HttpGet("me/sessions")]
        [ProducesResponseType(typeof(PagedResult<SessionHistoryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public Task<PagedResult<SessionHistoryModel>> GetSessions([FromQuery] SessionHistoryFilter filter,
            CancellationToken cancellationToken) => _sender.Send(new UserSessionsQuery(filter), cancellationToken);

        [HttpGet("me/sessions/{sessionId:guid}")]
        [ProducesResponseType(typeof(SessionHistoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<SessionHistoryModel>> GetSession(Guid sessionId, CancellationToken cancellationToken)
        {
            var session = await _sender.Send(new UserSessionByIdQuery(sessionId), cancellationToken);
            return session is null
                ? Problem(title: ValidationErrorMessages.HistoryNotFound, statusCode: StatusCodes.Status404NotFound)
                : Ok(session);
        }
    }
}
