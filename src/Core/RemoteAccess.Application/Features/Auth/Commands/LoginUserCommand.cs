using MediatR;
using RemoteAccess.Application.Features.Users.Models;

namespace RemoteAccess.Application.Features.Users.Commands;

public class LoginUserCommand : IRequest<AuthResult>
{
    public AuthDataInfo DataInfo { get; init; }
    public LoginUserCommand(AuthDataInfo dataInfo)
    {
        DataInfo = dataInfo;
    }
}
