local Timer = class("Timer",{});

local timers = {};

function LuaTimeTick(timespan)
    local _timers = timers;
    timers = {} --重置timer,运行完成后再把还要执行的timer放队列中
    -- print("current time count:",#_timers);
    for _,v in pairs(_timers) do
        if(v:_doTick(timespan) == true) then
            table.insert( timers,1,v); 
        end
    end
end

function Timer:ctor(timespan,callfun,looptimes,oncomplate)
    self._callfun = callfun;
    self._looptimes = looptimes;
    self._timespan = timespan;
    self._oncompate = oncomplate;
    self._isComplate = false;
    self._isStop = false;
    self._isPause = false;
    self._timerCounter = 0;
    -- 添加到timer 管理器中
    table.insert(timers,#timers + 1,self);
end

function Timer:stop()
    self._isStop = true;
end

function Timer:pause()
    self._isPause = true;
end

function Timer:start()
    self._startTime = getTime();
end

function Timer:_doTick(timespan)
    local function dotick(_self)
        -- print(">>>>>>",self._timerCounter,self._timespan,timespan)
        _self._timerCounter = _self._timerCounter + timespan;
        while(_self._timerCounter >  _self._timespan) do
            if(_self._isStop == true) then 
                _self:_doComplate(); 
                return false;
            end
            -- excecute.
            _self._timerCounter = _self._timerCounter - _self._timespan;
            _self._callfun();
            _self._looptimes = _self._looptimes - 1;
            if(_self._looptimes == 0) then _self._isComplate = true end
            -- check
            if(_self._isPause == true) then return true end
            if(_self._isComplate == true) then 
                _self:_doComplate();
                return false;
            end
        end
        return true;
    end
    return dotick(self); -- 是否可继续执行
end

function Timer:_doComplate()
    if(self._oncompate) then
        self._oncompate();
    end
end

function Timer.setTimeout(callfun,timeDelay)
    Timer.setInterval(timeDelay,callfun,1);
end

function Timer.setInterval(timespan,callfun,loop,oncomplate)
    local timer = Timer:new(timespan,callfun,loop,oncomplate);
    timer:start();
    return timer;
end

return Timer