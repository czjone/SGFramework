local WinBase = require("Base.WinBase");
local UIUtil = require("Base.UIUtil");
local Timer = require("Base.Timer");
local MainWin = class("MainWin",WinBase);

function MainWin:ctor(...)
    MainWin.super.ctor(self);
    self:setName("Assets/FZSG/dres/res/Wins/MainWin.prefab");
end

function MainWin:onUIReady()
    MainWin.super.onUIReady();
    self:doTimerCounter();
end

function MainWin:doTimerCounter()
    local waitSecond = 5;
    UIUtil.SetText(self.info,"5秒后开始游戏!");
    local _doTick = function()
        UIUtil.SetText(self.info, waitSecond .. "秒后开始游戏!");
        waitSecond = waitSecond - 1;
    end
    local _oncomplate = function()
        SGF.WinMgr.open("Window.LoadingWin");
    end
    local executeTimes = waitSecond + 1;-- 因为包含0,要多执行一次
    Timer.setInterval(1000,_doTick,executeTimes,_oncomplate); 
end

return MainWin;