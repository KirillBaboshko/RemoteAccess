if input.isOnline then
    redis.call('DEL', key('offline', input.deviceId))
else
    redis.call('SET', key('offline', input.deviceId), '1')
    finishDeviceSession(input.deviceId, 9) -- DeviceOffline
end
recordAvailability({input.deviceId})
return '{}'
