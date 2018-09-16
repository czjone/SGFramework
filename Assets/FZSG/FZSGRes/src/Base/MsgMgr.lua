local MsgMgr = class("MsgMgr",{})

local msgInstance = nil;

function MsgMgr.GetInstance()
    if(msgInstance == nil) then
        msgInstance = MsgMgr:new();
    end
    return msgInstance;
end

function MsgMgr:ctor()
    
end

function MsgMgr:send(type,...)

end

function MsgMgr:wait(type,fun)

end

return MsgMgr;