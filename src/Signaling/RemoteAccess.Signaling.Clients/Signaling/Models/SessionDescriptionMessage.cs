namespace RemoteAccess.Signaling.Clients.Signaling.Models;

/// <summary>
/// Повтор передачи использует те же SessionId и Sdp. Клиент должен распознавать дубликат
/// и не применять уже обработанный SDP повторно.
/// </summary>
public record SessionDescriptionMessage(Guid SessionId, string Sdp);
