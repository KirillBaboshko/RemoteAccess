local session, raw = read('session', input.id)
if not session then session = lastSession(input.id) end
if not session or (session.source.connectionId ~= input.connectionId and session.target.connectionId ~= input.connectionId) then
    return failure('SessionUnavailable')
end
if session.state ~= 0 and not isActive(session) then return cjson.encode({session = session}) end
if not raw then return failure('SessionUnavailable') end
return cjson.encode({session = finish(session, raw, 3, 1)})
