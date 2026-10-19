using MediatR;
using Microsoft.AspNetCore.Mvc;
using RemoteAccess.Application.Features.Users.Commands;
using RemoteAccess.Application.Features.Users.Models;

namespace RemoteAccess.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AccountsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    public Task<AuthResult> Register([FromBody] AuthDataInfo dataInfo, CancellationToken cancellationToken)
    {
        return _sender.Send(new RegisterUserCommand(dataInfo), cancellationToken);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    public Task<AuthResult> Login([FromBody] AuthDataInfo dataInfo, CancellationToken cancellationToken)
    {
        return _sender.Send(new LoginUserCommand(dataInfo), cancellationToken);
    }
}
