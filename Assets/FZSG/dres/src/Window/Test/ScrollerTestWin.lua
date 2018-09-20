local TestWinBase = require("Window.Test.TestWinBase");
local TestWin = class("TestWin",TestWinBase);

function TestWin:ctor()
    TestWin.super.ctor(self);
    self:setName("TestWin.Test.Scroller");
end

function TestWin:onUIReady()
    TestWin.super.onUIReady(self);
end

return TestWin