local WinBase = require("Base.WinBase");
local MessageBox = class("MessageBox",WinBase);

function MessageBox:ctor()
    MessageBox.super.ctor(self);
    self:setName("Assets/FZSG/dres/res/Wins/General/MessageBox.prefab");
end

function MessageBox:onUIReady()
    MessageBox.super.onUIReady(self);
    -- todo
end

return MessageBox;