local WinBase = require("Base.WinBase");
local HotUpdateWin = class("HotUpdateWin",WinBase);

function HotUpdateWin:ctor()
    MainWin.super.ctor(self);
    self:setName("wins.MainWin");
end

function HotUpdateWin:UIReady()
    HotUpdateWin.super.UIReady();
end

return HotUpdateWin;