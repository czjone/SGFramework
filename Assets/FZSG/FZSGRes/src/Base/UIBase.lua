local UIUtil = require("Base.UIUtil")
local UIBase = {}

function UIBase:ctor()
	self.uiroot = nil;
	self.data = nil;
	self.name = nil;
	self.parent = nil;

	self._showAni = nil;
    self._hideAni = nil;
	self._destoryAni = nil;
end

function UIBase:setName(nm)
	self.name = nm;
	self:loadUIAsy(function() 
		self.uiroot.name =self.name;
		self:onUIReady() 
	end)
end

function UIBase:loadUIAsy(callback)
	if(callback) then callback();end
end

-- function UIBase:trigger(type)

-- end

function UIBase:onUIReady()

end

function UIBase:onAddToStage()
	
end

function UIBase:onRemoveFromStage()

end

function UIBase:getParent()
	return self.parent;
end

function UIBase:addChild(n)
	UIUtil.AddChild(self,n);
end

function UIBase:removeFromParent()
	UIUtil.RemoveFromParent(self);
end

function UIBase:setRemoveAnimal(ani)
    self._showAni = ani;
end

function UIBase:setHideAnimal(ani)
    self._hideAni = ani;
end

function UIBase:setDestroyAnimal(ani)
    self._destoryAni = ani;
end

return UIBase;

