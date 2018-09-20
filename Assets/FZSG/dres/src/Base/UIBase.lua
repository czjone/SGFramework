local UIUtil = require("Base.UIUtil")
local UIBase = class("UIBase", {});
local Scroller = require("Base.Scroller");

function UIBase:ctor()
    self.uiroot = nil;
    self.name = nil;
    self.parent = nil;

    self._showAni = nil;
    self._hideAni = nil;
    self._destoryAni = nil;
end

function UIBase:setName(nm)
    self.name = nm;
    self:loadUIAsy(function()
        if(self.uiroot == nil) then
            error("load ui fail. name",nm);
        end
        print("load ui complate:",nm);
        self.uiroot.name = self.name;
        self:_addIndexSupprts();
        self:onUIReady()
    end)
end

function UIBase:_addIndexSupprts()
    local __uibaseIndex = self.__index;
    setmetatable(self, {
        __index = function(_table, key)
            local ret = __uibaseIndex[key];
            if (ret == nil) then --处理当前还没有缓存的子节点
                ret = self:_getUnityGameObjectWithUI(key);
                if(ret ~= nil) then
                    ret = self:_can2Scroller(ret); --如果是scroller就返回scroller对象
                else
                    error("not found lua object with key: ".. key);
                end
                self[key] = ret;
			end
            return ret;
        end
    })
end

--- self.scroller的时候返回的是一个scroller对象。可以直接当成scroller来处理。
function UIBase:_can2Scroller(go)
    local scrollerRect = go:GetComponent(typeof(CS.UnityEngine.UI.ScrollRect));
    if(scrollerRect ~= nil) then
        local scroller = Scroller:new(go);
        return scroller;
    end
    return go;
end

--- 查找unity gameobject 中的子对象.
function UIBase:_getUnityGameObjectWithUI(nm)
    return UIUtil.DeepFindChild(self.uiroot, nm);
end

function UIBase:loadUIAsy(callback)
    if (callback) then callback(); end
end

-- function UIBase:trigger(type)
-- end
function UIBase:onUIReady()

end

function UIBase:onAddToStage()

end

function UIBase:onRemoveFromStage()

end

function UIBase:addClick(id,callback)
    UIUtil.AddEvent(id,callback);
end

function UIBase:getParent()
    return self.parent;
end

function UIBase:addChild(n)
    UIUtil.AddChild(self, n);
end

function UIBase:removeFromParent()
    UIUtil.RemoveFromParent(self);
end

function UIBase:setRemoveAnimal(ani)
    self._showAni = ani;
end

function UIBase:setHideAnimal(ani)
    self._hideAni = ani;
end

function UIBase:setDestroyAnimal(ani)
    self._destoryAni = ani;
end

return UIBase;
