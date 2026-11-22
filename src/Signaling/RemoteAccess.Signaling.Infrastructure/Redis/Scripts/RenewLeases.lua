for _, id in ipairs(input.connections) do
    local connection = read('connection', id)
    if connection and connection.nodeId == node then
        redis.call('PEXPIRE', key('connection', id), connectionLease)
        if connection.hostDeviceId ~= '' then
            local host = read('host', connection.hostDeviceId)
            if host and host.connectionId == id then redis.call('PEXPIRE', key('host', host.deviceId), connectionLease) end
        end
        for _, sessionId in ipairs(redis.call('SMEMBERS', key('connection-sessions', id))) do
            local session, raw = read('session', sessionId)
            if session and session.state == 1 and canRenew(session) then renewSession(session, raw) end
        end
    end
end
return '{}'
