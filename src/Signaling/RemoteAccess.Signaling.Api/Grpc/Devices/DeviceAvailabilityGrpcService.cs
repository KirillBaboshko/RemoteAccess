using FluentValidation;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using RemoteAccess.Contracts.Signaling.Devices;
using RemoteAccess.Signaling.Application.Features.Devices.Commands;
using RemoteAccess.Signaling.Application.Features.Devices.Queries;
using ConnectionStatus = RemoteAccess.Signaling.Domain.Models.Devices.DeviceConnectionStatus;

namespace RemoteAccess.Signaling.Api.Grpc.Devices;

[Authorize]
public sealed class DeviceAvailabilityGrpcService : DeviceAvailability.DeviceAvailabilityBase
{
    private readonly ISender _sender;
    private readonly IValidator<GetDeviceAvailabilityRequest> _availabilityValidator;
    private readonly IValidator<DeviceStatusChangedRequest> _statusValidator;

    public DeviceAvailabilityGrpcService(ISender sender, IValidator<GetDeviceAvailabilityRequest> availabilityValidator,
        IValidator<DeviceStatusChangedRequest> statusValidator)
    {
        _sender = sender;
        _availabilityValidator = availabilityValidator;
        _statusValidator = statusValidator;
    }

    public override async Task<GetDeviceAvailabilityResponse> GetMany(GetDeviceAvailabilityRequest request, ServerCallContext context)
    {
        await _availabilityValidator.ValidateAndThrowAsync(request, context.CancellationToken);
        var devices = await _sender.Send(new DeviceAvailabilityQuery(request.DeviceIds.Select(Guid.Parse).Distinct().ToArray()),
            context.CancellationToken);
        var response = new GetDeviceAvailabilityResponse();
        response.Devices.AddRange(devices.Select(device => new DeviceAvailabilityItem
        {
            DeviceId = device.DeviceId.ToString(),
            Status = device.Status switch
            {
                ConnectionStatus.Offline => DeviceConnectionStatus.Offline,
                ConnectionStatus.Online => DeviceConnectionStatus.Online,
                ConnectionStatus.Busy => DeviceConnectionStatus.Busy,
                _ => DeviceConnectionStatus.Unspecified
            }
        }));
        return response;
    }

    public override async Task<DeviceStatusChangedResponse> NotifyStatusChanged(DeviceStatusChangedRequest request, ServerCallContext context)
    {
        await _statusValidator.ValidateAndThrowAsync(request, context.CancellationToken);
        var authorization = context.GetHttpContext().Request.Headers.Authorization.ToString();
        var token = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? authorization[7..] : string.Empty;
        await _sender.Send(new NotifyDeviceStatusChangedCommand(Guid.Parse(request.DeviceId), token), context.CancellationToken);
        return new DeviceStatusChangedResponse();
    }
}
