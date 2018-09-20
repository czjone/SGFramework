local WinBase = require("Base.WinBase");
local TestWin = class("TestWin",WinBase);

local ItemBase = require("Base.ItemBase");
local MenuItem = class("MenuItem",ItemBase)
local UIUtil = require("Base.UIUtil");

function TestWin:ctor()
    TestWin.super.ctor(self);
    self:setName("TestWin.MainWin");
end

function TestWin:onUIReady()
    TestWin.super.onUIReady(self);
    local scroller = self.MenuScroller;
    scroller:setLayoutType(ScrollerLayoutType.G)
    scroller:setItem(MenuItem);
    scroller:setData({
        {text = "Scroll View Test", testWin = "Window.Test.ScrollerTestWin"},
    });
end
----------------------------------------------------------------
-- MenuItem
function MenuItem:ctor(data)
    MenuItem.super.ctor(self,data);
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
    UIUtil.SetText(uilabel,self.data.text);
    self:addClick(uilabel,function() 
        SGF.WinMgr.Open(self.data.testWin);
    end);
end

return TestWin