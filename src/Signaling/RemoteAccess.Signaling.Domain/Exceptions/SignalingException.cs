namespace RemoteAccess.Signaling.Domain.Exceptions;

public sealed class SignalingException : Exception
{
    public SignalingException(string message) : base(message)
    {
    }
}
