local WinBase = require("Base.WinBase");
local MainWin = class("MainWin",WinBase);
local UIUtil = SGF.UIUtil;

function MainWin:ctor()
    MainWin.super.ctor(self);
    self:setName("Assets/FZSG/FZSGRes/res/wins/MainWin.prefab");
end

function MainWin:UIReady()
    MainWin.super.UIReady();
    -- SGF.Timer.setInterval(3,function()
    --     UIUtil.SetText(self.Info,"222222222");
    -- end,3);
    print("口口kkkkkkkkkkkkkkkkkk");
    UIUtil.SetText(self.Info,"222222222");
end

return MainWin;