redis.call('SET', key('connection', input.connectionId), cjson.encode({
    userId = input.userId, connectionId = input.connectionId, nodeId = node, hostDeviceId = ''
}), 'PX', connectionLease)
return '{}'
