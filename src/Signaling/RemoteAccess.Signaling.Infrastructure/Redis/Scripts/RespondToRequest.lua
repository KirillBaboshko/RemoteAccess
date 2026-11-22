local session, raw = read('session', input.id)
if not session or session.target.connectionId ~= input.connectionId or session.state ~= 0
    or session.expiresAt <= now or not canRenew(session) then return failure('RequestUnavailable') end
if input.accepted then
    session.state, session.acceptedAt, session.version = 1, now, session.version + 1
    save(session, raw, now + sessionLease)
else
    session = finish(session, raw, 2, 0)
end
return cjson.encode({session = session})
