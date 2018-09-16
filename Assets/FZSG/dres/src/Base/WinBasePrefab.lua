local UIBase = require("Base.UIBase");
local UIUtil = require("Base.UIUtil");
local PrefabWinBase = class("PrefabWinBase",UIBase);

function PrefabWinBase:ctor()
    PrefabWinBase.super.ctor(self);
end

function PrefabWinBase:setName(nm)
    PrefabWinBase.super.setName(self,nm);
end

function PrefabWinBase:loadUIAsy(callback)
    self.uiroot = UIUtil.LoadPrefab(self.name);
    PrefabWinBase.super.loadUIAsy(self,callback);
end

function PrefabWinBase:onUIReady()
    PrefabWinBase.super.onUIReady(self);
end

function PrefabWinBase:show()

end

function PrefabWinBase:hide()

end

function PrefabWinBase:close()

end

return PrefabWinBase;