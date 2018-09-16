local WinMgr = {}
local UIUtil = require("Base.UIUtil");
local UIRoot = SGF.UIRoot; -- 在main中定义的

function WinMgr.open(win,...)
    if(type(win) == "string")then
        local win = require(win):new();
        assert(UIRoot ~= nil, "UI Root is nil");
        assert(win ~= nil, "create windows fail.",win);
        UIUtil.AddChild(UIRoot,win);
        win:show();
    else
        error("use not support open window model.");
    end
end

return WinMgr;
