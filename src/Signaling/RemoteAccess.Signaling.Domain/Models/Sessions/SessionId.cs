using RemoteAccess.Common.Domain;

namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public sealed record SessionId(Guid Value) : StronglyTypedId<Guid>(Value);
