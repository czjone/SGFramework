local WinBase = require("Base.WinBase")
local TestWinBase = class("TestWinBase",WinBase)

function TestWinBase:ctor()
    TestWinBase.super.ctor(self);
end

function TestWinBase:onUIReady()
    TestWinBase.super.onUIReady(self);
    self:setBaseUI();
end

function TestWinBase:setBaseUI()
    self:addClick(self.back,function() 
        print("close windows");
    end)
end
return TestWinBase;