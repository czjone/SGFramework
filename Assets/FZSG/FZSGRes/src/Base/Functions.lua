local socket = require("socket");

_G["print"] = function(...) end --这样就不会调用到xlua的打印
function setPrintPower(showLog)
    if(showLog == true) then
        _G["print"] = function ( ... )
            arg = { ... } ;
            local val =  "SGF LUA:";
            for k,v in pairs(arg) do  val = val .. v .."    "; end
            CS.UnityEngine.Debug.Log(val);
        end 

    else
        _G["print"] = function(...) end
    end
end
-- function printf(fmt, ...)
--     print(string.format(tostring(fmt), ...))
-- end

function assert(exp,...)
    if(exp == false) then
        print(...);
    end
end

function tostring_table(o, s, isArray)
    s = s or ''
    local custumPairs = pairs
    if isArray then
        custumPairs = ipairs
    end
    if type(o) == 'table' then
        s = s .. '{'
        for k, v in custumPairs(o) do
            local str = k .. '='
            --if(type(k) == 'number')then
            --   str = ""
            --end
            if
                type(v) == 'number' or type(v) == 'function' or type(v) == 'boolean' or type(v) == 'userdata' or
                    type(v) == 'nil'
             then
                s = s .. str .. tostring(v) .. ','
            elseif type(v) == 'string' then
                s = s .. str .. string.format('%q', v) .. ','
            else
                s = s .. str
                s = s .. tostring_table(v)
                s = s .. ','
            end
        end
        s = s .. '}'
    end
    return s
end

function table.tostring(t, usen)
    if t == nil then
        return nil
    end

    if type(t) ~= 'table' then
        return tostring(t)
    end
    if t.tostring and type(t.tostring) == 'function' then
        return t:tostring()
    end
    local str, _type = ''
    local nchar = usen and '\n' or ''
    for k, v in pairs(t) do
        _type = type(v)
        if _type == 'nil' then
            str = str .. k .. '=nil, ' .. nchar
        elseif _type == 'table' then
            if (v.class) then
                str =
                    str ..
                    k ..
                        '={' ..
                            (v.tostring and v.tostring(v) or ('class=' .. tostring(v.class.__classname))) .. '}, ' .. nchar
            else
                str = str .. k .. '={' .. (v.tostring and v.tostring(v) or table.tostring(v, usen)) .. '}, ' .. nchar
            end
        else
            str = str .. k .. '=' .. tostring(v) .. ', ' .. nchar
        end
    end
    return str
end

function table.nums(t)
    local count = 0
    for k, v in pairs(t) do
        count = count + 1
    end
    return count
end

local setmetatableindex_
setmetatableindex_ = function(t, index)
    -- if type(t) == "userdata" then
    --     local peer = tolua.getpeer(t)
    --     if not peer then
    --         peer = {}
    --         tolua.setpeer(t, peer)
    --     end
    --     setmetatableindex_(peer, index)
    -- else
    local mt = getmetatable(t)
    if not mt then
        mt = {}
    end
    if not mt.__index then
        mt.__index = index
        setmetatable(t, mt)
    elseif mt.__index ~= index then
        setmetatableindex_(mt, index)
    end
    -- end
end
setmetatableindex = setmetatableindex_

function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local newObject = {}
        lookup_table[object] = newObject
        for key, value in pairs(object) do
            newObject[_copy(key)] = _copy(value)
        end
        return setmetatable(newObject, getmetatable(object))
    end
    return _copy(object)
end

function class(classname, ...)
    local cls = {__classname = classname}

    local supers = {...}
    for _, super in ipairs(supers) do
        local superType = type(super)
        -- assert(
        --     superType == "nil" or superType == "table" or superType == "function",
        --     string.format('class() - create class "%s" with invalid super class type "%s"', classname, superType)
        -- )

        if superType == "function" then
            -- assert(
            --     cls.__create == nil,
            --     string.format('class() - create class "%s" with more than one creating function', classname)
            -- )
            -- if super is function, set it to __create
            cls.__create = super
        elseif superType == "table" then
            if super[".isclass"] then
                -- super is native class
                -- assert(
                --     cls.__create == nil,
                --     string.format(
                --         'class() - create class "%s" with more than one creating function or native class',
                --         classname
                --     )
                -- )
                cls.__create = function()
                    return super:create()
                end
            else
                -- super is pure lua class
                cls.__supers = cls.__supers or {}
                cls.__supers[#cls.__supers + 1] = super
                if not cls.super then
                    -- set first super pure lua class as class.super
                    cls.super = super
                end
            end
        else
            error(string.format('class() - create class "%s" with invalid super type', classname), 0)
        end
    end

    cls.__index = cls
    if not cls.__supers or #cls.__supers == 1 then
        setmetatable(cls, {__index = cls.super})
    else
        setmetatable(
            cls,
            {
                __index = function(_, key)
                    local supers = cls.__supers
                    for i = 1, #supers do
                        local super = supers[i]
                        if super[key] then
                            return super[key]
                        end
                    end
                end
            }
        )
    end

    if not cls.ctor then
        -- add default constructor
        cls.ctor = function()
        end
    end
    cls.new = function(...)
        local instance
        if cls.__create then
            instance = cls.__create(...)
        else
            instance = {}
        end
        setmetatableindex(instance, cls)
        instance.class = cls
        instance:ctor(...)
        return instance
    end
    cls.create = function(_, ...)
        return cls.new(...)
    end

    return cls
end

local iskindof_
iskindof_ = function(cls, name)
    local __index = rawget(cls, "__index")
    if type(__index) == "table" and rawget(__index, "__classname") == name then
        return true
    end

    if rawget(cls, "__classname") == name then
        return true
    end

    if (cls.class and cls.class.__classname == name) then
        return true
    end

    local __supers = rawget(cls, "__supers")
    if not __supers then
        return false
    end
    for _, super in ipairs(__supers) do
        if iskindof_(super, name) then
            return true
        end
    end
    return false
end

function checknumber(value, base)
    return tonumber(value, base) or 0
end

function checkint(value)
    return math.round(checknumber(value))
end

function checkbool(value)
    return (value ~= nil and value ~= false)
end

function checktable(value)
    if type(value) ~= "table" then value = {} end
    return value
end

function instanceof(obj, classname)
    return iskindof(obj, classname)
end

function iskindof(obj, classname)
    local t = type(obj)
    if t ~= "table" and t ~= "userdata" then
        return false
    end

    local mt
    -- if t == "userdata" then
    --     if tolua.iskindof(obj, classname) then
    --         return true
    --     end
    --     mt = tolua.getpeer(obj)
    --     mt = (mt and mt.class) and mt.class or mt
    -- else
    mt = obj.class and obj.class or getmetatable(obj)
    -- end
    if mt then
        return iskindof_(mt, classname)
    end
    return false
end

function import(moduleName, currentModuleName)
    local currentModuleNameParts
    local moduleFullName = moduleName
    local offset = 1

    while true do
        if string.byte(moduleName, offset) ~= 46 then -- .
            moduleFullName = string.sub(moduleName, offset)
            if currentModuleNameParts and #currentModuleNameParts > 0 then
                moduleFullName = table.concat(currentModuleNameParts, ".") .. "." .. moduleFullName
            end
            break
        end
        offset = offset + 1

        if not currentModuleNameParts then
            if not currentModuleName then
                local n, v = debug.getlocal(3, 1)
                currentModuleName = v
            end

            currentModuleNameParts = string.split(currentModuleName, ".")
        end
        table.remove(currentModuleNameParts, #currentModuleNameParts)
    end

    return require(moduleFullName)
end


string._htmlspecialchars_set = {}
string._htmlspecialchars_set["&"] = "&amp;"
string._htmlspecialchars_set["\""] = "&quot;"
string._htmlspecialchars_set["'"] = "&#039;"
string._htmlspecialchars_set["<"] = "&lt;"
string._htmlspecialchars_set[">"] = "&gt;"

function string.htmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, k, v)
    end
    return input
end

function string.restorehtmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, v, k)
    end
    return input
end

function string.nl2br(input)
    return string.gsub(input, "\n", "<br />")
end

function string.text2html(input)
    input = string.gsub(input, "\t", "    ")
    input = string.htmlspecialchars(input)
    input = string.gsub(input, " ", "&nbsp;")
    input = string.nl2br(input)
    return input
end

function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter=='') then return false end
    local pos,arr = 0, {}
    -- for each divider found
    for st,sp in function() return string.find(input, delimiter, pos, true) end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

function string.ltrim(input)
    return string.gsub(input, "^[ \t\n\r]+", "")
end

function string.rtrim(input)
    return string.gsub(input, "[ \t\n\r]+$", "")
end

function string.trim(input)
    input = string.gsub(input, "^[ \t\n\r]+", "")
    return string.gsub(input, "[ \t\n\r]+$", "")
end

function string.ucfirst(input)
    return string.upper(string.sub(input, 1, 1)) .. string.sub(input, 2)
end

local function urlencodechar(char)
    return "%" .. string.format("%02X", string.byte(char))
end
function string.urlencode(input)
    -- convert line endings
    input = string.gsub(tostring(input), "\n", "\r\n")
    -- escape all characters but alphanumeric, '.' and '-'
    input = string.gsub(input, "([^%w%.%- ])", urlencodechar)
    -- convert spaces to "+" symbols
    return string.gsub(input, " ", "+")
end

function string.urldecode(input)
    input = string.gsub (input, "+", " ")
    input = string.gsub (input, "%%(%x%x)", function(h) return string.char(checknumber(h,16)) end)
    input = string.gsub (input, "\r\n", "\n")
    return input
end

function string.utf8len(input)
    local len  = string.len(input)
    local left = len
    local cnt  = 0
    local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
    while left ~= 0 do
        local tmp = string.byte(input, -left)
        local i   = #arr
        while arr[i] do
            if tmp >= arr[i] then
                left = left - i
                break
            end
            i = i - 1
        end
        cnt = cnt + 1
    end
    return cnt
end

function string.formatnumberthousands(num)
    local formatted = tostring(checknumber(num))
    local k
    while true do
        formatted, k = string.gsub(formatted, "^(-?%d+)(%d%d%d)", '%1,%2')
        if k == 0 then break end
    end
    return formatted
end

function getTime()
    return socket.gettime();
end