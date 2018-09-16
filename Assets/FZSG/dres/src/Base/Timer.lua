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

function Timer:ctor(timespan,callfun,looptimes)
    self._callfun = callfun;
    self._looptimes = looptimes;
    self._timespan = timespan;
    self._isComplate = false;
    self._isStop = false;
    self._isPause = true;
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
    local _self = self;
    local function dotick()
        self._timerCounter = self._timerCounter + timespan;
        while(self._timerCounter >  self._timespan) do
            -- excecute.
            self._timerCounter = self._timerCounter - self._timespan;
            self._looptimes = self._looptimes - 1;
            self._callfun();
            if(self._looptimes == 0) then self._isComplate = true end
            -- check
            if(_self._isStop == true) then return false end
            if(_self._isPause == true) then return true end
            if(_self._isComplate == true) then return false end
        end
    end
    -- do
    local ret,err = pcall(dotick);
    if(err) then return false end
    return ret; -- 是否可继续执行
end

function Timer.setTimeout(callfun,timeDelay)
    Timer.setInterval(timeDelay,callfun,1);
end

function Timer.setInterval(timespan,callfun,loop)
    local timer = Timer:new(timespan,callfun,loop);
    timer:start();
end

return Timer