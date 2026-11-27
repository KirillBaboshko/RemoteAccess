using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Api.Grpc.Interceptors;

public sealed class GrpcExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcExceptionInterceptor> _logger;

    public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger) => _logger = logger;

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException error)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                string.Join("; ", error.Errors.Select(failure => failure.ErrorMessage))));
        }
        catch (UnauthorizedAccessException)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, ValidationErrorMessages.UserRequired));
        }
        catch (SignalingException error)
        {
            throw new RpcException(new Status(StatusCode.Unavailable, error.Message));
        }
        catch (Exception error) when (error is not RpcException && error is not OperationCanceledException)
        {
            _logger.LogError(error, "Ошибка внутреннего запроса {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.Internal, ValidationErrorMessages.UnexpectedError));
        }
    }
}
