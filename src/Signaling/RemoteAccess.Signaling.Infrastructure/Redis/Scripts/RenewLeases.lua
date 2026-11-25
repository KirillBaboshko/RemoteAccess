for _, registration in ipairs(input.connections) do
    local id = registration.connectionId
    if redis.call('EXISTS', key('closed', id)) == 0 then
        local connection = read('connection', id)
        if not connection then
            connection = {userId = registration.userId, connectionId = id, nodeId = node,
                hostDeviceId = registration.hostDeviceId}
            redis.call('SET', key('connection', id), cjson.encode(connection), 'PX', connectionLease)
        end
        if connection.nodeId == node and connection.userId == registration.userId then
            redis.call('PEXPIRE', key('connection', id), connectionLease)
            if connection.hostDeviceId ~= '' then
                local deviceId = connection.hostDeviceId
                local host = read('host', deviceId)
                if not host then
                    host = {userId = connection.userId, connectionId = id, deviceId = deviceId}
                    redis.call('SET', key('host', deviceId), cjson.encode(host), 'PX', connectionLease)
                    recordAvailability({deviceId})
                end
                if host.connectionId == id then
                    redis.call('PEXPIRE', key('host', deviceId), connectionLease)
                    redis.call('ZADD', hostDeadlines, now + connectionLease, deviceId)
                end
            end
            for _, sessionId in ipairs(redis.call('SMEMBERS', key('connection-sessions', id))) do
                local session, raw = read('session', sessionId)
                if session and isActive(session) and canRenew(session) then renewSession(session, raw) end
            end
        end
    end
end
return '{}'
