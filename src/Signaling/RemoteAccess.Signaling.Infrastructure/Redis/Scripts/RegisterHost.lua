local connection = read('connection', input.connectionId)
if not connection or connection.userId ~= input.userId then return failure('ConnectionRequired') end
if connection.hostDeviceId ~= '' and connection.hostDeviceId ~= input.deviceId then
    return failure('ConnectionAlreadyRegistered')
end
local host = read('host', input.deviceId)
if host and host.connectionId ~= input.connectionId and read('connection', host.connectionId) then
    return failure('HostAlreadyRegistered')
end
connection.hostDeviceId = input.deviceId
redis.call('SET', key('connection', input.connectionId), cjson.encode(connection), 'PX', connectionLease)
redis.call('SET', key('host', input.deviceId), cjson.encode(input), 'PX', connectionLease)
return '{}'
