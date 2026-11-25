local node = ARGV[1]
local connectionLease, sessionLease, grace = tonumber(ARGV[2]), tonumber(ARGV[3]), tonumber(ARGV[4])
local negotiationTimeout = tonumber(ARGV[5])
local connectionTimeout, recoveryTimeout = tonumber(ARGV[6]), tonumber(ARGV[7])
local input = cjson.decode(ARGV[8])
local clock = redis.call('TIME')
local now = tonumber(clock[1]) * 1000 + math.floor(tonumber(clock[2]) / 1000)
local prefixes = { connection = KEYS[1], host = KEYS[2], session = KEYS[3], busy = KEYS[4], ['connection-sessions'] = KEYS[5] }
local deadlines, events = KEYS[6], KEYS[7]
local function key(kind, id) return prefixes[kind] .. id end
local function read(kind, id)
    local value = redis.call('GET', key(kind, id))
    if value then return cjson.decode(value), value end
end
local function failure(code) return cjson.encode({error = code}) end
local function expireLater(k, deadline)
    if redis.call('PTTL', k) < deadline - now then redis.call('PEXPIREAT', k, deadline) end
end
local function releaseDevice(device, sessionId)
    local k = key('busy', device)
    if redis.call('GET', k) == sessionId then redis.call('DEL', k) end
end
local function record(session)
    redis.call('XADD', events, '*', 'session', cjson.encode(session))
end
local function isActive(session)
    return session.state == 1 or session.state == 5 or session.state == 6
end
local function participantRole(session, userId, connectionId)
    if session.source.userId == userId and session.source.connectionId == connectionId then return 'source' end
    if session.target.userId == userId and session.target.connectionId == connectionId then return 'target' end
end
local function callerConnected(userId, connectionId)
    local connection = read('connection', connectionId)
    return connection and connection.userId == userId and connection.nodeId == node
end
local function negotiationDeadline(session)
    if session.state ~= 1 or session.negotiationState == 3 then return nil end
    local deadline = session.negotiationExpiresAt
    if not deadline or deadline == cjson.null then return session.acceptedAt + negotiationTimeout end
    return deadline
end
local function activeDeadline(session)
    local negotiation = negotiationDeadline(session)
    if negotiation then return negotiation, 4 end -- NegotiationTimeout
    if session.state == 1 and session.negotiationState == 3 then
        local deadline = session.connectionExpiresAt
        -- Старые записи не содержат отдельного срока подключения.
        if not deadline or deadline == cjson.null then
            deadline = session.acceptedAt + negotiationTimeout + connectionTimeout
        end
        return deadline, 5 -- ConnectionTimeout
    end
    if session.state == 6 then return session.recoveryExpiresAt, 8 end -- RecoveryTimeout
end
local function leaseDeadline(session)
    local deadline = now + sessionLease
    local active = activeDeadline(session)
    if active then return math.min(deadline, active) end
    return deadline
end
local function save(session, previous, deadline)
    session.updatedAt = now
    local raw = cjson.encode(session)
    if previous then redis.call('ZREM', deadlines, previous) end
    redis.call('SET', key('session', session.id), raw, 'PXAT', deadline + grace)
    for _, participant in ipairs({session.source, session.target}) do
        redis.call('SET', key('busy', participant.deviceId), session.id, 'PXAT', deadline + grace)
        local index = key('connection-sessions', participant.connectionId)
        redis.call('SADD', index, session.id)
        expireLater(index, deadline + grace)
    end
    redis.call('ZADD', deadlines, deadline, raw)
    record(session)
end
local function finish(session, raw, state, reason)
    redis.call('DEL', key('session', session.id))
    redis.call('ZREM', deadlines, raw)
    for _, participant in ipairs({session.source, session.target}) do
        releaseDevice(participant.deviceId, session.id)
        redis.call('SREM', key('connection-sessions', participant.connectionId), session.id)
    end
    session.state, session.endReason, session.endedAt, session.updatedAt = state, reason, now, now
    session.version = session.version + 1
    record(session)
    return session
end
local function canRenew(session)
    return read('connection', session.source.connectionId) and read('connection', session.target.connectionId)
        and redis.call('GET', key('busy', session.source.deviceId)) == session.id
        and redis.call('GET', key('busy', session.target.deviceId)) == session.id
end
local function renewSession(session, raw)
    local active = activeDeadline(session)
    if active and active <= now then return end
    local deadline = leaseDeadline(session)
    redis.call('PEXPIREAT', key('session', session.id), deadline + grace)
    for _, participant in ipairs({session.source, session.target}) do
        redis.call('PEXPIREAT', key('busy', participant.deviceId), deadline + grace)
        expireLater(key('connection-sessions', participant.connectionId), deadline + grace)
    end
    redis.call('ZADD', deadlines, deadline, raw)
end
