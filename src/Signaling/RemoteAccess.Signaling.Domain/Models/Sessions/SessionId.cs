using RemoteAccess.Common.Domain;

namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public sealed record SessionId(Guid Value) : StronglyTypedId<Guid>(Value), IComparable<SessionId>
{
    public int CompareTo(SessionId? other) => other is null ? 1 : Value.CompareTo(other.Value);
}
