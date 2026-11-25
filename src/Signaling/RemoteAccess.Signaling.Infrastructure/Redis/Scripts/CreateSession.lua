local source = input.source
local connection = read('connection', source.connectionId)
if not connection or connection.userId ~= source.userId then return failure('ConnectionRequired') end
if source.deviceId == input.targetDeviceId then return failure('SelfConnection') end
if redis.call('EXISTS', key('offline', source.deviceId), key('offline', input.targetDeviceId)) > 0 then
    return failure('DeviceOffline')
end
local sourceHost = read('host', source.deviceId)
local sourceAgent = sourceHost and read('connection', sourceHost.connectionId)
if not sourceAgent or sourceAgent.userId ~= source.userId or sourceAgent.hostDeviceId ~= source.deviceId then
    return failure('DeviceOffline')
end
local target = read('host', input.targetDeviceId)
local targetAgent = target and read('connection', target.connectionId)
if not targetAgent or targetAgent.userId ~= target.userId or targetAgent.hostDeviceId ~= target.deviceId then
    return failure('DeviceOffline')
end
if source.connectionId == target.connectionId then return failure('SelfConnection') end
if redis.call('EXISTS', key('busy', source.deviceId), key('busy', target.deviceId)) > 0 then
    return failure('DeviceBusy')
end
local session = {
    id = input.id, source = source, target = target, createdAt = now,
    expiresAt = now + input.pendingTimeout, state = 0, version = 1,
    acceptedAt = cjson.null, endedAt = cjson.null, endReason = cjson.null,
    negotiationState = 0, negotiationExpiresAt = cjson.null,
    sourceConnectionState = 0, targetConnectionState = 0,
    connectionExpiresAt = cjson.null, recoveryExpiresAt = cjson.null, connectedAt = cjson.null
}
save(session, nil, leaseDeadline(session))
return cjson.encode({session = session})
