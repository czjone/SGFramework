local UIBase = require("Base.UIBase");
local ItemBase = class("ItemBase",UIBase)

function ItemBase:ctor()
    ItemBase.spure.ctor(self);
end

--- 数据发生变化时调用
function ItemBase:onDatachanged()

end

--- 索引发生变化时调用
function ItemBase:onIndexChanged()

end

return UIBase;