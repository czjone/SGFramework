local WinBase = require("Base.WinBase");
local LoadingWin = class("LoadingWin",WinBase);

function LoadingWin:ctor( )
    LoadingWin.super.ctor(self);
    self:setName("Assets/FZSG/dres/res/Wins/General/LoadingWin.prefab");
end

return LoadingWin;