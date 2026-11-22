local session, raw = read('session', input.id)
if not session or (session.source.connectionId ~= input.connectionId and session.target.connectionId ~= input.connectionId) then
    return failure('SessionUnavailable')
end
return cjson.encode({session = finish(session, raw, 3, 1)})
