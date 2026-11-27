using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteAccess.Common.Data;
using RemoteAccess.Common.Extensions;
using RemoteAccess.Signaling.Data.DataProviders.Sessions;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Infrastructure.Database;

namespace RemoteAccess.Signaling.Infrastructure.DataProviders;

public sealed class SessionHistoryDataProvider : DataProviderBase, ISessionHistoryDataProvider
{
    private readonly IMapper _mapper;

    public SessionHistoryDataProvider(SignalingDbContext context, IMapper mapper) : base(context) => _mapper = mapper;

    public async Task<PagedResult<SessionHistoryModel>> GetHistory(Guid userId, SessionHistoryFilter filter,
        CancellationToken cancellationToken)
    {
        var query = ParticipantHistory(userId)
            .WhereIf(filter.DeviceId.HasValue,
                session => session.SourceDeviceId == filter.DeviceId || session.TargetDeviceId == filter.DeviceId)
            .WhereIf(filter.State.HasValue, session => session.State == filter.State)
            .WhereIf(filter.From.HasValue, session => session.CreatedAt >= filter.From!.Value.UtcDateTime)
            .WhereIf(filter.To.HasValue, session => session.CreatedAt < filter.To!.Value.UtcDateTime);
        var totalCount = await query.CountAsync(cancellationToken);
        var sessions = await query.OrderByDescending(session => session.CreatedAt)
            .ThenByDescending(session => session.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);
        return new(_mapper.Map<List<SessionHistoryModel>>(sessions), totalCount, filter.Page, filter.PageSize);
    }

    public async Task<SessionHistoryModel?> GetById(Guid userId, Guid sessionId, CancellationToken cancellationToken)
    {
        var id = new SessionId(sessionId);
        var session = await ParticipantHistory(userId).SingleOrDefaultAsync(session => session.Id == id, cancellationToken);
        return _mapper.Map<SessionHistoryModel?>(session);
    }

    private IQueryable<SessionHistory> ParticipantHistory(Guid userId) => Query<SessionHistory>()
        .Where(session => session.SourceUserId == userId || session.TargetUserId == userId)
        .Where(session => session.State == SessionState.Ended || session.State == SessionState.Rejected
            || session.State == SessionState.Expired);
}
