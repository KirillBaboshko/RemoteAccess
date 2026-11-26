using System.Text.Json.Serialization;

namespace RemoteAccess.Signaling.Clients.Ice.Models;

public record IceServerModel(string[] Urls,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Username = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Credential = null);
