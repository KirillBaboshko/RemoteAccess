local session = read('session', input.id)
if not session then session = lastSession(input.id) end
if not session or (session.source.userId ~= input.userId and session.target.userId ~= input.userId) then
    return failure('SessionUnavailable')
end
return cjson.encode({session = session})
