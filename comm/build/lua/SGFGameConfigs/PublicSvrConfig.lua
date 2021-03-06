-- automatically generated by the FlatBuffers compiler, do not modify

-- namespace: SGFGameConfigs

local flatbuffers = require('flatbuffers')

local PublicSvrConfig = {} -- the module
local PublicSvrConfig_mt = {} -- the class metatable

function PublicSvrConfig.New()
    local o = {}
    setmetatable(o, {__index = PublicSvrConfig_mt})
    return o
end
function PublicSvrConfig.GetRootAsPublicSvrConfig(buf, offset)
    local n = flatbuffers.N.UOffsetT:Unpack(buf, offset)
    local o = PublicSvrConfig.New()
    o:Init(buf, n + offset)
    return o
end
function PublicSvrConfig_mt:Init(buf, pos)
    self.view = flatbuffers.view.New(buf, pos)
end
function PublicSvrConfig_mt:Host()
    local o = self.view:Offset(4)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function PublicSvrConfig_mt:Port()
    local o = self.view:Offset(6)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function PublicSvrConfig_mt:Db()
    local o = self.view:Offset(8)
    if o ~= 0 then
        return self.view:String(o + self.view.pos)
    end
end
function PublicSvrConfig.Start(builder) builder:StartObject(3) end
function PublicSvrConfig.AddHost(builder, host) builder:PrependUOffsetTRelativeSlot(0, host, 0) end
function PublicSvrConfig.AddPort(builder, port) builder:PrependUOffsetTRelativeSlot(1, port, 0) end
function PublicSvrConfig.AddDb(builder, db) builder:PrependUOffsetTRelativeSlot(2, db, 0) end
function PublicSvrConfig.End(builder) return builder:EndObject() end

return PublicSvrConfig -- return the module