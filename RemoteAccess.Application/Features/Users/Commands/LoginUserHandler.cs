using MediatR;
using RemoteAccess.Application.Features.Users.Models;
using RemoteAccess.Data.DataProviders.Users.Model;
using RemoteAccess.Domain.Models;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Domain.Services.Token;
using AutoMapper;
using Duende.IdentityModel;

namespace RemoteAccess.Application.Features.Users.Commands;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResult>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenIssuer _tokenIssuer;
    private readonly IMapper _mapper;

    public LoginUserHandler(IUsersRepository usersRepository, ITokenIssuer tokenIssuer, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _tokenIssuer = tokenIssuer;
        _mapper = mapper;
    }

    public async Task<AuthResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByEmailAsync(request.DataInfo.Login, cancellationToken);
        if (!user.CheckPassword(request.DataInfo.Password))
        {
            throw new InvalidOperationException("Не правильный пароль");
        }

        var token = await _tokenIssuer.CreateToken(user.Id, user.UserEmail.Email);

        var model = _mapper.Map<UserShortModel>(user);

        return new AuthResult(model, token);
    }
}
