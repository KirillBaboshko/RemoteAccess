local updates = {}
for _, raw in ipairs(redis.call('ZRANGEBYSCORE', deadlines, '-inf', now, 'LIMIT', 0, 100)) do
    local session = cjson.decode(raw)
    local current = redis.call('GET', key('session', session.id))
    if not current or current == raw then
        local deadline, reason = activeDeadline(session)
        if deadline and deadline <= now then
            table.insert(updates, cjson.encode(finish(session, raw, 3, reason)))
        elseif current and isActive(session) and canRenew(session) then
            renewSession(session, raw)
        else
            local state, reason = 3, 2
            if session.state == 0 then state, reason = 4, 3 end
            table.insert(updates, cjson.encode(finish(session, raw, state, reason)))
        end
    else
        redis.call('ZREM', deadlines, raw)
    end
end
return '{"sessions":[' .. table.concat(updates, ',') .. ']}'
