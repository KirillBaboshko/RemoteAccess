using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Api.Hubs;

public sealed class SignalingExceptionFilter : IHubFilter
{
    private readonly ILogger<SignalingExceptionFilter> _logger;

    public SignalingExceptionFilter(ILogger<SignalingExceptionFilter> logger) => _logger = logger;

    public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch (ValidationException error)
        {
            throw new HubException(string.Join("; ", error.Errors.Select(e => e.ErrorMessage)));
        }
        catch (SignalingException error)
        {
            throw new HubException(error.Message);
        }
        catch (Exception error) when (error is not HubException && error is not OperationCanceledException)
        {
            _logger.LogError(error, "Ошибка метода сигнализации {Method}", invocationContext.HubMethodName);
            throw new HubException(ValidationErrorMessages.UnexpectedError);
        }
    }
}
