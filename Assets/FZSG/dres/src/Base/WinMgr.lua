local WinMgr = {}
local UIUtil = require("Base.UIUtil");
local UIRoot = SGFGame.UIRoot; -- 在main中定义的

WinMgr._wins = {} --;当前在内存中的窗口

function WinMgr.Open(win,...)
    WinMgr.CreateWin(win,...);
end

function WinMgr.CreateWin(win,...)
    if(type(win) == "string")then
        local win = require(win):new(...);
        if(win == nil) then error("create windows fail.") end;
        WinMgr.AddToStage(win)
    else
        error("use not support open window model.");
    end
end

function WinMgr.AddToStage(winObj)
    if(UIRoot == nil) then error("UI Root is nil") end
    UIUtil.AddChild(UIRoot,winObj);
    table.insert( WinMgr._wins, #(WinMgr._wins) + 1,winObj);
    winObj:show();
    WinMgr.CheckWeight(); --- 处理窗口之间的层级关系，后台关系，显示关系
end

function WinMgr.CheckWeight()
    if(#(WinMgr._wins) >= 2) then
        local win =WinMgr._wins[1];
        win:close();
        table.remove( WinMgr._wins, 1 ); -- 删除第一个
    end
end

return WinMgr;
