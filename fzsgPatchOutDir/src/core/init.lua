local  Init = {}

require("core.functions")

_G["SGF"] = {
    WinMgr = require("core.ui.winmgr");
    Winbase = require("core.ui.winbase");
};

return Init;