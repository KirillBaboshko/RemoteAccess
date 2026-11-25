local availability = {}
for _, deviceId in ipairs(input.deviceIds) do
    local host = read('host', deviceId)
    local connection = host and read('connection', host.connectionId)
    local online = connection and connection.userId == host.userId and connection.hostDeviceId == deviceId
    local status = 0 -- Offline
    if redis.call('EXISTS', key('offline', deviceId)) == 1 then status = 0
    elseif redis.call('EXISTS', key('busy', deviceId)) == 1 then status = 2 -- Busy
    elseif online then status = 1 end -- Online
    table.insert(availability, cjson.encode({deviceId = deviceId, status = status}))
end
return '{"availability":[' .. table.concat(availability, ',') .. ']}'
