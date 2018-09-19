local UIBase = require("Base.UIBase")
local Scroller = class("Scroller",UIBase);

function Scroller:ctor()
    Scroller.super.ctor(self);
    self._item = nil;
    self._itemsChanged = false;
end

function Scroller:setItem(item)
    if(self._item == item) then
        self._item = item;
        self._itemsChanged = true;
    end
end

function Scroller:setData(data)
    
end

function Scroller:_refUIList()
    
end

return Scroller;