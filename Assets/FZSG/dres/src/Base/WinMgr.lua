local WinMgr = {}
local UIUtil = require("Base.UIUtil");
local UIRoot = SGFGame.UIRoot; -- 在main中定义的

function WinMgr.open(win,...)
    if(type(win) == "string")then
        local win = require(win):new(...);
        if(UIRoot == nil) then error("UI Root is nil") end
        if(win == nil) then error("create windows fail.") end;
        UIUtil.AddChild(UIRoot,win);
        win:show();
    else
        error("use not support open window model.");
    end
end

return WinMgr;
