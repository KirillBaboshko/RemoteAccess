local updates = {}
for _, deviceId in ipairs(redis.call('ZRANGEBYSCORE', hostDeadlines, '-inf', now, 'LIMIT', 0, 100)) do
    local host = read('host', deviceId)
    local connection = host and read('connection', host.connectionId)
    if host and connection and connection.hostDeviceId == deviceId then
        local ttl = math.min(redis.call('PTTL', key('host', deviceId)),
            redis.call('PTTL', key('connection', host.connectionId)))
        redis.call('ZADD', hostDeadlines, now + ttl, deviceId)
    else
        redis.call('DEL', key('host', deviceId))
        redis.call('ZREM', hostDeadlines, deviceId)
        recordAvailability({deviceId})
        local ended = finishDeviceSession(deviceId, 2)
        if ended then table.insert(updates, cjson.encode(ended)) end
    end
end
for _, raw in ipairs(redis.call('ZRANGEBYSCORE', deadlines, '-inf', now, 'LIMIT', 0, 100)) do
    local session = cjson.decode(raw)
    local current = redis.call('GET', key('session', session.id))
    if not current or current == raw then
        local deadline, reason = activeDeadline(session)
        if deadline and deadline <= now then
            table.insert(updates, cjson.encode(finish(session, raw, session.state == 0 and 4 or 3, reason)))
        elseif current and canRenew(session) then
            renewSession(session, raw)
        else
            table.insert(updates, cjson.encode(finish(session, raw, 3, 2)))
        end
    else
        redis.call('ZREM', deadlines, raw)
    end
end
return '{"sessions":[' .. table.concat(updates, ',') .. ']}'
