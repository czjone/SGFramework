local Scroller = class("Scroller",{});
local UIUtil = require("Base.UIUtil")

ScrollerLayoutType = {
    None = 0,
    H = 1,
    V = 2,
    G = 3,
}

function Scroller:ctor(go)
    self.uiroot = go;
    self._itemCls = nil
    self._layoutType = nil;
    self._layoutGroup = nil;
    self._content = UIUtil.DeepFindChild(go,"Content");
    self._viewport = UIUtil.DeepFindChild(go,"Viewport");
    self:_checkSroller(self._content);
    self:setLayoutType(ScrollerLayoutType.None)
end

function Scroller:setItem(item)
    if(self._itemCls ~= item) then
        self._itemCls = item;
        self:_refAllItems();
    end
end

function Scroller:addItem(data)
    local item = self._itemCls:new(data);
    self:_addItemLayoutSupports(item);
    UIUtil.AddChild(self._content,item); 
end

function Scroller:_addItemLayoutSupports(item)
    item.uiroot:AddComponent(typeof(CS.UnityEngine.UI.LayoutElement));
end

function Scroller:addItems(datas)
    self.datas = datas;
    for k,v in pairs(datas) do
        self:addItem(v);
    end
end

function Scroller:removeItem()

end

function Scroller:removeItems()

end

function Scroller:setData(data)
    self:addItems(data);
end

function Scroller:_checkSroller(_content)
    if(_content == nil) then error("scroller not validity！") end
    local t = typeof(CS.UnityEngine.UI.ContentSizeFitter);
    local contentSizeFitter = _content:GetComponent(t);
    -- 自动化大小
    if(contentSizeFitter == nil) then
        self._contentSizeFitter = _content:AddComponent(t);
        local csf = self._contentSizeFitter;
        csf.horizontalFit = CS.UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
		csf.verticalFit =  CS.UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
    end
end

--- 
-- 0：不排序
-- 1：横向
-- 2：纵向
-- 3:表格模式
function Scroller:setLayoutType(lt)
    lt = lt or ScrollerLayoutType.None;
    if(lt == self._layoutType) then return end
    -- 为了逻辑简单，没有做过多的性能上的考虑，
    -- 也不会有智障有事没事的就调用函数玩吧。
    if(self._layoutGroup ~= nil) then
        self._content:RemoveComponent(self._layoutGroup);
    end
    -- 横向和纵向表格
    if(lt == ScrollerLayoutType.H) then
        self._content:AddComponent(typeof(CS.UnityEngine.UI.HorizontalLayoutGroup));
    elseif(lt == ScrollerLayoutType.V) then
        self._content:AddComponent(typeof(CS.UnityEngine.UI.VerticalLayoutGroup));
    elseif(lt == ScrollerLayoutType.G) then
        self._content:AddComponent(typeof(CS.UnityEngine.UI.GridLayoutGroup));
    end
end

function Scroller:_refAllItems()
    if(self.datas ~= nil and self._itemCls ~= nil) then
        -- TODO:刷新全部已创建的节点
    end
end

return Scroller;