local WinBase = require("Base.WinBase");
local UIUtil = require("Base.UIUtil");
local MainWin = class("MainWin",WinBase);

function MainWin:ctor()
    MainWin.super.ctor(self);
    self:setName("Assets/FZSG/dres/res/wins/MainWin.prefab");
end

function MainWin:onUIReady()
    MainWin.super.onUIReady();
    -- SGF.Timer.setInterval(3,function()
    --     UIUtil.SetText(self.Info,"222222222");
    -- end,3);
    UIUtil.SetText(self.info,"5秒后开始游戏!");
end

return MainWin;