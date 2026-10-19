using MediatR;
using RemoteAccess.Application.Features.Users.Models;

namespace RemoteAccess.Application.Features.Users.Commands;

/// <summary>
/// Команда регистрации пользователя
/// </summary>
public class RegisterUserCommand: IRequest<AuthResult>
{ 
    public AuthDataInfo DataInfo { get; init; }
    public RegisterUserCommand(AuthDataInfo dataInfo)
    {
        DataInfo = dataInfo;
    }
}

