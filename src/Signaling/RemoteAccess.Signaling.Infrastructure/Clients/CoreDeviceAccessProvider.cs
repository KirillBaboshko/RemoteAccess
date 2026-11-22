using Grpc.Core;
using Microsoft.Extensions.Options;
using RemoteAccess.Contracts.Core.Devices;
using RemoteAccess.Signaling.Application.Features.Devices.Models;
using RemoteAccess.Signaling.Application.Services;
using RemoteAccess.Signaling.Domain.Exceptions;
using RemoteAccess.Signaling.Infrastructure.Settings;

namespace RemoteAccess.Signaling.Infrastructure.Clients;

public sealed class CoreDeviceAccessProvider : IDeviceAccessProvider
{
    private readonly DeviceAccess.DeviceAccessClient _client;
    private readonly CoreApiSettings _settings;

    public CoreDeviceAccessProvider(DeviceAccess.DeviceAccessClient client, IOptions<CoreApiSettings> settings)
    {
        _client = client;
        _settings = settings.Value;
    }

    public async Task<DeviceAccessModel?> GetAccess(Guid deviceId, string accessToken, CancellationToken cancellationToken)
    {
        try
        {
            var headers = new Metadata { { "authorization", "Bearer " + accessToken } };
            var response = await _client.GetAccessAsync(new GetDeviceAccessRequest { DeviceId = deviceId.ToString() },
                headers, deadline: DateTime.UtcNow.Add(_settings.Timeout), cancellationToken: cancellationToken);
            return new DeviceAccessModel(response.IsOwner, response.Name);
        }
        catch (RpcException error) when (error.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
        catch (RpcException error) when (error.StatusCode is StatusCode.Unauthenticated or StatusCode.PermissionDenied)
        {
            throw new SignalingException(ValidationErrorMessages.TokenRejected);
        }
        catch (RpcException error) when (error.StatusCode == StatusCode.Cancelled && cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(error.Message, error, cancellationToken);
        }
        catch (RpcException)
        {
            throw new SignalingException(ValidationErrorMessages.CoreUnavailable);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new SignalingException(ValidationErrorMessages.CoreUnavailable);
        }
    }
}
