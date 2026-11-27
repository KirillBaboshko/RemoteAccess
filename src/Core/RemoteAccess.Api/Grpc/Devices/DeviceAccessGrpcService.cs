using FluentValidation;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using RemoteAccess.Common.Extensions;
using RemoteAccess.Application.Features.Devices.Queries;
using RemoteAccess.Contracts.Core.Devices;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Api.Grpc.Devices;

[Authorize]
public sealed class DeviceAccessGrpcService : DeviceAccess.DeviceAccessBase
{
    private readonly ISender _sender;
    private readonly IValidator<GetDeviceAccessRequest> _validator;

    public DeviceAccessGrpcService(ISender sender, IValidator<GetDeviceAccessRequest> validator)
    {
        _sender = sender;
        _validator = validator;
    }

    public override async Task<GetDeviceAccessResponse> GetAccess(GetDeviceAccessRequest request, ServerCallContext context)
    {
        await _validator.ValidateAndThrowAsync(request, context.CancellationToken);
        var access = await _sender.Send(new DeviceAccessQuery(Guid.Parse(request.DeviceId),
            context.GetHttpContext().User.GetUserId()), context.CancellationToken);

        if (access is null)
            throw new RpcException(new Status(StatusCode.NotFound, ValidationErrorMessages.DeviceNotFound));

        return new GetDeviceAccessResponse
        {
            IsOwner = access.IsOwner,
            Name = access.Name,
            IsOnline = access.IsOnline
        };
    }
}
