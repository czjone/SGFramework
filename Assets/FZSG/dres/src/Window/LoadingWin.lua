local WinBase = require("Base.WinBase");
local LoadingWin = class("LoadingWin",WinBase);

function LoadingWin:ctor( )
    LoadingWin.super.ctor(self);
    self:setName("Wins.General.LoadingWin");
end

function LoadingWin:onUIReady()
    LoadingWin.super.onUIReady(self);
    self:start();
end

function LoadingWin:start()

end

return LoadingWin;