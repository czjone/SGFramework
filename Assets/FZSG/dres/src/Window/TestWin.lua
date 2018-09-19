local WinBase = require("Base.WinBase");
local TestWin = class("TestWin",WinBase);

local ItemBase = require("Base.ItemBase");
local MenuItem = class("MenuItem",ItemBase)

function TestWin:ctor()
    TestWin.super.ctor(self);
    self:setName("TestWin.MainWin");
end

function TestWin:onUIReady()
    TestWin.super.onUIReady(self);
    local scroller = self.MenuScroller;
    scroller:setItem(MenuItem);
    scroller:setData({
        {text = "UI Test"},
        {text = "Event Test"}
    });
end
----------------------------------------------------------------
-- MenuItem
function MenuItem:ctor()
    MenuItem.super.ctor(self);
    self:setName("TestWin.MenuItem");
end

function MenuItem:onUIReady()
    MenuItem.super.onUIReady(self);
    local data = self.data;
    self:_refUI(data);
end

function MenuItem:onDataChanged()
    MenuItem.super.onDataChanged(self);
    local data = self.data;
    self:_refUI(data)
end

function MenuItem:_refUI(data)
    local uilabel = self.VisibleText;
    UIUtile.SetText(uilabel,self.data.text);
end

return TestWin