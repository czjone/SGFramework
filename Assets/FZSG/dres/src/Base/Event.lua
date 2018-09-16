local Event = class("Event",{})

function Event:ctor()
    Event.super.ctor(self);
    self._events = {};
end

function Event:addEventlistner(type,callfunc,obj)
    table.insert( self._events , #(self._events) + 1, {
        type = type,
        callfunc = callfunc,
        obj = obj,
        isOnce = false,
    });
end

function Event:addEventlistnerOnce(type,callfunc,obj)
    table.insert( self._events , #(self._events) + 1, {
        type = type,
        callfunc = callfunc,
        obj = obj,
        isOnce = true,
    });
end

function Event:dispatch(type)
    local delKeys = {};
    for k,v in pairs(self._events) do
        if(v.type == type) then
            v.callfunc(v.obj,self);
            if(v.isOnce == true) then
                table.insert(delKeys,#delKeys + 1, k); 
            end
        end
    end
    --从后向前删除.
   for i=#(delKeys),1 do
        table.remove( self._events, delKeys[i]);
   end
end

return Event;