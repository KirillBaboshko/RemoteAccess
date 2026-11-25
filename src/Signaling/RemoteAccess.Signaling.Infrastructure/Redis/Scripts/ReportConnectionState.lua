local session, raw = read('session', input.id)
if not session or not isActive(session) then return failure('SessionUnavailable') end
local role = participantRole(session, input.userId, input.connectionId)
if not role then return failure('WebRtcSenderInvalid') end
if not callerConnected(input.userId, input.connectionId) then return failure('ConnectionRequired') end
if not canRenew(session) then return failure('SessionUnavailable') end
local deadline, reason = activeDeadline(session)
if deadline and deadline <= now then
    return failure(reason == 4 and 'NegotiationExpired' or 'ConnectionExpired')
end

local state = input.state
if type(state) ~= 'number' or state < 0 or state > 5 or state ~= math.floor(state) then
    return failure('ConnectionStateInvalid')
end
local field = role .. 'ConnectionState'
local previous = session[field] or 0
if (state == 2 and session.negotiationState ~= 3) or (state == 0 and previous ~= 0) then
    return failure('ConnectionStateTransitionInvalid')
end
if previous == state then return cjson.encode({session = session}) end
session[field] = state

if state == 4 or state == 5 then
    return cjson.encode({session = finish(session, raw, 3, state == 4 and 6 or 7)})
end
if session.sourceConnectionState == 2 and session.targetConnectionState == 2 then
    session.state = 5 -- Connected
    if not session.connectedAt or session.connectedAt == cjson.null then session.connectedAt = now end
    session.connectionExpiresAt, session.recoveryExpiresAt = cjson.null, cjson.null
elseif session.state == 5 or session.state == 6 then
    session.state = 6 -- Reconnecting
    if not session.recoveryExpiresAt or session.recoveryExpiresAt == cjson.null then
        session.recoveryExpiresAt = now + recoveryTimeout
    end
end
session.version = session.version + 1
save(session, raw, leaseDeadline(session))
return cjson.encode({session = session})
