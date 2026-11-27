using System.Text.Json.Serialization;
using RemoteAccess.Contracts.Signaling.Sessions;

namespace RemoteAccess.Data.DataProviders.Sessions.Model;

public sealed record SessionHistoryModel(
    Guid SessionId,
    Guid SourceUserId,
    Guid TargetUserId,
    Guid SourceDeviceId,
    Guid TargetDeviceId,
    [property: JsonConverter(typeof(JsonStringEnumConverter<HistoryState>))] HistoryState State,
    string? EndReason,
    DateTime CreatedAt,
    DateTime? AcceptedAt,
    DateTime? ConnectedAt,
    DateTime? EndedAt,
    double? DurationSeconds);
