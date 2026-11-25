using MediatR;
using RemoteAccess.Signaling.Domain.Models.Devices;

namespace RemoteAccess.Signaling.Application.Features.Devices.Queries;

public record DeviceAvailabilityQuery(Guid[] DeviceIds) : IRequest<IReadOnlyList<DeviceAvailability>>;
