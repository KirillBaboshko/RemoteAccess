local connection = read('connection', input.connectionId)
local updates = {}
if connection and connection.nodeId ~= node then return failure('ConnectionRequired') end
-- Не позволяем уже снятому снимку продления восстановить закрытое соединение.
redis.call('SET', key('closed', input.connectionId), '1', 'PX', connectionLease * 2)
if connection and connection.hostDeviceId ~= '' then
    local host = read('host', connection.hostDeviceId)
    if host and host.connectionId == input.connectionId then
        redis.call('DEL', key('host', connection.hostDeviceId))
        redis.call('ZREM', hostDeadlines, connection.hostDeviceId)
        recordAvailability({connection.hostDeviceId})
        local ended = finishDeviceSession(connection.hostDeviceId, 2)
        if ended then table.insert(updates, cjson.encode(ended)) end
    end
end
redis.call('DEL', key('connection', input.connectionId))
local index = key('connection-sessions', input.connectionId)
for _, id in ipairs(redis.call('SMEMBERS', index)) do
    local session, raw = read('session', id)
    if session then table.insert(updates, cjson.encode(finish(session, raw, 3, 2))) end
end
redis.call('DEL', index)
return '{"sessions":[' .. table.concat(updates, ',') .. ']}'
