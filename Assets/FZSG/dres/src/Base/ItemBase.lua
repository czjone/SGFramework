local UIBase = require("Base.UIBase");
local ItemBase = class("ItemBase",UIBase)
local UIUtil = require("Base.UIUtil");

function ItemBase:ctor(data)
    self.data = data;
    ItemBase.super.ctor(self);
end

function ItemBase:loadUIAsy(callback)
    self.uiroot = UIUtil.LoadPrefab(self.name);
    ItemBase.super.loadUIAsy(self,callback);
end

--- 数据发生变化时调用
function ItemBase:onDatachanged()

end

--- 索引发生变化时调用
function ItemBase:onIndexChanged()

end

return ItemBase;