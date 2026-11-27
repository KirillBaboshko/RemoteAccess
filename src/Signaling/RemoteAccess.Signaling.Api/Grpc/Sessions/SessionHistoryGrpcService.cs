using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using RemoteAccess.Contracts.Signaling.Sessions;
using RemoteAccess.Common.Extensions;
using RemoteAccess.Signaling.Application.Features.Sessions.Queries;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;
using RemoteAccess.Signaling.Domain.Exceptions;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using HistoryService = RemoteAccess.Contracts.Signaling.Sessions.SessionHistory;

namespace RemoteAccess.Signaling.Api.Grpc.Sessions;

[Authorize]
public sealed class SessionHistoryGrpcService : HistoryService.SessionHistoryBase
{
    private readonly ISender _sender;

    public SessionHistoryGrpcService(ISender sender) => _sender = sender;

    public override async Task<SessionHistoryPage> GetList(GetSessionHistoryRequest request, ServerCallContext context)
    {
        var userId = GetCallerId(context);
        var filter = new SessionHistoryFilter
        {
            DeviceId = request.HasDeviceId ? ParseId(request.DeviceId, ValidationErrorMessages.DeviceRequired) : null,
            State = !request.HasState ? null : request.State switch
            {
                HistoryState.Ended => SessionState.Ended,
                HistoryState.Rejected => SessionState.Rejected,
                HistoryState.Expired => SessionState.Expired,
                _ => throw new RpcException(new Status(StatusCode.InvalidArgument, ValidationErrorMessages.HistoryStateInvalid))
            },
            From = ReadTimestamp(request.From),
            To = ReadTimestamp(request.To),
            Page = request.Page,
            PageSize = request.PageSize
        };
        var result = await _sender.Send(new SessionHistoryQuery(userId, filter), context.CancellationToken);
        var page = new SessionHistoryPage { TotalCount = result.TotalCount, Page = result.Page, PageSize = result.PageSize };
        page.Items.AddRange(result.Items.Select(ToItem));
        return page;
    }

    public override async Task<SessionHistoryItem> GetById(GetSessionHistoryByIdRequest request, ServerCallContext context)
    {
        var userId = GetCallerId(context);
        var id = ParseId(request.SessionId, ValidationErrorMessages.SessionRequired);
        var session = await _sender.Send(new SessionHistoryByIdQuery(userId, id), context.CancellationToken);
        if (session is null)
            throw new RpcException(new Status(StatusCode.NotFound, ValidationErrorMessages.HistoryNotFound));
        return ToItem(session);
    }

    private static Guid GetCallerId(ServerCallContext context) =>
        context.GetHttpContext().User.TryGetNonEmptyUserId(out var userId)
            ? userId : throw new UnauthorizedAccessException(ValidationErrorMessages.UserRequired);

    private static Guid ParseId(string value, string error) => Guid.TryParse(value, out var id)
        ? id : throw new RpcException(new Status(StatusCode.InvalidArgument, error));

    private static DateTimeOffset? ReadTimestamp(Timestamp? value)
    {
        try
        {
            return value?.ToDateTimeOffset();
        }
        catch (InvalidOperationException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ValidationErrorMessages.RequestInvalid));
        }
    }

    private static SessionHistoryItem ToItem(SessionHistoryModel session)
    {
        var item = new SessionHistoryItem
        {
            SessionId = session.SessionId.ToString(),
            SourceUserId = session.SourceUserId.ToString(),
            TargetUserId = session.TargetUserId.ToString(),
            SourceDeviceId = session.SourceDeviceId.ToString(),
            TargetDeviceId = session.TargetDeviceId.ToString(),
            State = session.State switch
            {
                SessionState.Ended => HistoryState.Ended,
                SessionState.Rejected => HistoryState.Rejected,
                SessionState.Expired => HistoryState.Expired,
                _ => throw new InvalidOperationException(ValidationErrorMessages.HistoryStateInvalid)
            },
            CreatedAt = Timestamp.FromDateTime(session.CreatedAt),
            AcceptedAt = session.AcceptedAt?.ToTimestamp(),
            ConnectedAt = session.ConnectedAt?.ToTimestamp(),
            EndedAt = session.EndedAt?.ToTimestamp()
        };
        if (session.EndReason is { } reason) item.EndReason = reason.ToString();
        if (session.DurationSeconds is { } duration) item.DurationSeconds = duration;
        return item;
    }
}
