local session, raw = read('session', input.id)
if not session then session = lastSession(input.id) end
if not session or session.target.connectionId ~= input.connectionId then return failure('RequestUnavailable') end
if session.state ~= 0 then
    if (input.accepted and session.acceptedAt and session.acceptedAt ~= cjson.null)
        or (not input.accepted and session.state == 2) then return cjson.encode({session = session}) end
    return failure('RequestUnavailable')
end
if not raw or session.expiresAt <= now or not canRenew(session) then return failure('RequestUnavailable') end
if redis.call('EXISTS', key('offline', session.source.deviceId), key('offline', session.target.deviceId)) > 0 then
    return failure('DeviceOffline')
end
if input.accepted then
    session.state, session.acceptedAt, session.version = 1, now, session.version + 1
    session.negotiationState, session.negotiationExpiresAt = 1, now + negotiationTimeout
    save(session, raw, leaseDeadline(session))
else
    session = finish(session, raw, 2, 0)
end
return cjson.encode({session = session})
