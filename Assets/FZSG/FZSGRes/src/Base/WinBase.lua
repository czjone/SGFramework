local WinBasePrefab = require("Base.WinBasePrefab");
local UIUtil = require("Base.UIUtil");
local WinBase = class("WinBase",WinBasePrefab);

function WinBase:ctor()
    WinBase.super.ctor(self);
end

function WinBase:UIReady()
    WinBase.super.UIReady(self);
end

function WinBase:setName(nm)
    WinBase.super.setName(self,nm);
end

function WinBase:show()
    
end

function WinBase:hide()

end

function WinBase:close(destroy)

end

return WinBase;