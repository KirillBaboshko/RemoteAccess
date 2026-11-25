using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RemoteAccess.Contracts.Signaling.Devices;
using RemoteAccess.Data.DataProviders.Devices;
using RemoteAccess.Infrastructure.Settings;
using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Infrastructure.Clients;

public sealed class SignalingDeviceAvailabilityProvider : IDeviceAvailabilityProvider
{
    private readonly DeviceAvailability.DeviceAvailabilityClient _client;
    private readonly SignalingApiSettings _settings;
    private readonly IHttpContextAccessor _http;
    private readonly ILogger<SignalingDeviceAvailabilityProvider> _logger;

    public SignalingDeviceAvailabilityProvider(DeviceAvailability.DeviceAvailabilityClient client,
        IOptions<SignalingApiSettings> settings, IHttpContextAccessor http,
        ILogger<SignalingDeviceAvailabilityProvider> logger)
    {
        _client = client;
        _settings = settings.Value;
        _http = http;
        _logger = logger;
    }

    public async Task<IReadOnlyDictionary<Guid, DeviceStatus>> GetAvailability(
        IReadOnlyCollection<Guid> deviceIds, CancellationToken cancellationToken)
    {
        var result = new Dictionary<Guid, DeviceStatus>();
        if (deviceIds.Count == 0) return result;
        try
        {
            var headers = GetHeaders();
            var deadline = DateTime.UtcNow.Add(_settings.Timeout);
            foreach (var batch in deviceIds.Distinct().Chunk(DeviceAvailabilityLimits.BatchSize))
            {
                var request = new GetDeviceAvailabilityRequest();
                request.DeviceIds.AddRange(batch.Select(id => id.ToString()));
                var response = await _client.GetManyAsync(request, headers, deadline, cancellationToken);
                var expected = batch.ToHashSet();
                foreach (var item in response.Devices)
                {
                    if (!Guid.TryParse(item.DeviceId, out var id) || !expected.Remove(id))
                        throw new DeviceAvailabilityUnavailableException();
                    var status = item.Status switch
                    {
                        DeviceConnectionStatus.Offline => DeviceStatus.Offline,
                        DeviceConnectionStatus.Online => DeviceStatus.Online,
                        DeviceConnectionStatus.Busy => DeviceStatus.Busy,
                        _ => throw new DeviceAvailabilityUnavailableException()
                    };
                    result.Add(id, status);
                }
                if (expected.Count != 0) throw new DeviceAvailabilityUnavailableException();
            }
            return result;
        }
        catch (RpcException error) when (error.StatusCode == StatusCode.Cancelled && cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(error.Message, error, cancellationToken);
        }
        catch (RpcException error)
        {
            throw new DeviceAvailabilityUnavailableException(error);
        }
    }

    public async Task NotifyStatusChanged(Guid deviceId, CancellationToken cancellationToken)
    {
        try
        {
            await _client.NotifyStatusChangedAsync(new DeviceStatusChangedRequest { DeviceId = deviceId.ToString() },
                GetHeaders(), DateTime.UtcNow.Add(_settings.Timeout), cancellationToken);
        }
        catch (Exception error) when (error is RpcException or DeviceAvailabilityUnavailableException)
        {
            _logger.LogWarning(error, $"Не удалось сообщить об изменении статуса устройства {deviceId}");
        }
    }

    private Metadata GetHeaders()
    {
        var authorization = _http.HttpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization)) throw new DeviceAvailabilityUnavailableException();
        return new Metadata { { "authorization", authorization } };
    }
}
