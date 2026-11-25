local session, raw = read('session', input.id)
if not session or not isActive(session) then return failure('SessionUnavailable') end
local role = participantRole(session, input.userId, input.connectionId)
if not role then return failure('WebRtcSenderInvalid') end
if not callerConnected(input.userId, input.connectionId) then
    return failure('ConnectionRequired')
end
if not canRenew(session) then return failure('SessionUnavailable') end
local deadline, reason = activeDeadline(session)
if deadline and deadline <= now then
    return failure(reason == 4 and 'NegotiationExpired' or 'ConnectionExpired')
end
local phase = session.negotiationState
if not phase or phase == 0 then phase = 1 end
if input.messageType == 0 then
    if role ~= 'source' then return failure('WebRtcSenderInvalid') end
    if session.offerHash and session.offerHash == input.descriptionHash then
        return cjson.encode({session = session})
    end
    if phase ~= 1 then return failure('WebRtcSequenceInvalid') end
    if not input.descriptionHash or input.descriptionHash == cjson.null then return failure('WebRtcSequenceInvalid') end
    session.offerHash = input.descriptionHash
    session.negotiationState = 2
    session.negotiationExpiresAt = deadline
elseif input.messageType == 1 then
    if role ~= 'target' then return failure('WebRtcSenderInvalid') end
    if session.answerHash and session.answerHash == input.descriptionHash then
        return cjson.encode({session = session})
    end
    if phase ~= 2 then return failure('WebRtcSequenceInvalid') end
    if not input.descriptionHash or input.descriptionHash == cjson.null then return failure('WebRtcSequenceInvalid') end
    session.answerHash = input.descriptionHash
    session.negotiationState, session.negotiationExpiresAt = 3, cjson.null
    session.connectionExpiresAt = now + connectionTimeout
elseif input.messageType == 2 then
    if phase ~= 2 and phase ~= 3 then return failure('WebRtcSequenceInvalid') end
    return cjson.encode({session = session})
else
    return failure('WebRtcSequenceInvalid')
end
session.version = session.version + 1
save(session, raw, leaseDeadline(session))
return cjson.encode({session = session})
