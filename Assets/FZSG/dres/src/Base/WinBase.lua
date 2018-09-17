local WinBasePrefab = require("Base.WinBasePrefab");
local UIUtil = require("Base.UIUtil");
local WinBase = class("WinBase",WinBasePrefab);

function WinBase:ctor()
    WinBase.super.ctor(self);
    self._weight = 0;
end

function WinBase:onUIReady()
    WinBase.super.onUIReady(self);
end

function WinBase:setName(nm)
    WinBase.super.setName(self,nm);
end

function WinBase:show()
    
end

function WinBase:hide()

end

function WinBase:close(destroy)
    destroy = destroy or true;
    if(destroy == true) then
        CS.UnityEngine.GameObject.DestroyImmediate(self.uiroot)
    end
end

function WinBase:getWeight()
    return self._weight;
end

function WinBase:setWeight(weight)
    self._weight = weight;
end

return WinBase;