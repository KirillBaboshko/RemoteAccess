using MediatR;
using RemoteAccess.Signaling.Clients.Ice;
using RemoteAccess.Signaling.Clients.Ice.Models;

namespace RemoteAccess.Signaling.Application.Features.Ice.Queries;

public sealed class GetIceConfigurationHandler : IRequestHandler<GetIceConfigurationQuery, IceConfigurationModel>
{
    private readonly IIceConfigurationProvider _provider;

    public GetIceConfigurationHandler(IIceConfigurationProvider provider) => _provider = provider;

    public Task<IceConfigurationModel> Handle(GetIceConfigurationQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_provider.GetConfiguration(request.Caller.UserId));
    }
}
