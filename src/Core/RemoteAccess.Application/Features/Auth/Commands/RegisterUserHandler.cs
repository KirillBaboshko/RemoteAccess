using MediatR;
using RemoteAccess.Data.DataProviders.Users.Model;
using RemoteAccess.Application.Features.Users.Models;
using RemoteAccess.Domain.Models.Users;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Domain.Services.Token;
using RemoteAccess.Common.Data;
using AutoMapper;

namespace RemoteAccess.Application.Features.Users.Commands;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResult>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenIssuer _tokenIssuer;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
        IUsersRepository usersRepository, 
        ITokenIssuer tokenIssuer, 
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _tokenIssuer = tokenIssuer;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.DataInfo.Login, request.DataInfo.Password);
        await _usersRepository.CreateAsync(user, cancellationToken);
        
        // Сохранение изменений через UnitOfWork
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = await _tokenIssuer.CreateToken(user.Id, user.UserEmail.Email);

        var model = _mapper.Map<UserShortModel>(user);

        return new AuthResult(model, token);
    }
}
