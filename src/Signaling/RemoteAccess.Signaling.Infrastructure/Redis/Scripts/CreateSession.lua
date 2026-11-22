local source = input.source
local connection = read('connection', source.connectionId)
if not connection or connection.userId ~= source.userId then return failure('ConnectionRequired') end
if source.deviceId == input.targetDeviceId then return failure('SelfConnection') end
local target = read('host', input.targetDeviceId)
if not target or not read('connection', target.connectionId) then return failure('DeviceOffline') end
if source.connectionId == target.connectionId then return failure('SelfConnection') end
if redis.call('EXISTS', key('busy', source.deviceId), key('busy', target.deviceId)) > 0 then
    return failure('DeviceBusy')
end
local session = {
    id = input.id, source = source, target = target, createdAt = now,
    expiresAt = now + input.pendingTimeout, state = 0, version = 1,
    acceptedAt = cjson.null, endedAt = cjson.null, endReason = cjson.null
}
save(session, nil, session.expiresAt)
return cjson.encode({session = session})
