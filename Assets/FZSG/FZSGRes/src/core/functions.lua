
function print( ... )
    arg = { ... } ;
    local val =  "";
    for k,v in pairs(arg) do  val = val .. v ..","; end
    CS.UnityEngine.Debug.Log(val);
end

function class(classname, super)
    local superType = type(super)
    local cls
    --如果提供的super既不是函数，也不是table，那么就直接将super当做不存在
    if superType ~= "function" and superType ~= "table" then
        superType = nil
        super = nil
    end
    --如果有提供super
    if superType == "function" or (super and super.__ctype == 1) then
        -- 定义一个table，它将作为类的定义
        cls = {}
        --如果super是table，那么将super中定义的字段先全部拷贝到cls中
        --然后将__create方法指定为super的__create方法
        if superType == "table" then
            -- copy fields from super
            for k,v in pairs(super) do cls[k] = v end
            cls.__create = super.__create
            cls.super    = super
        else 
        --这里提供的super时函数，那么也就是用这个方法来构造对象，那么直接将__create方法指向super
            cls.__create = super
        end
        --提供一个空的ctor构造函数
        cls.ctor    = function() end
        cls.__cname = classname
        cls.__ctype = 1
        --定义.new(...)方法，他用户构建对象，首先是调用__create方法来构建一个table对象，然后将cls里面定义的字段全部拷贝到创建的对象中
        --接下来在调用ctor构造方法
        function cls.new(...)
            local instance = cls.__create(...)
            -- copy fields from class to native object
            for k,v in pairs(cls) do instance[k] = v end
            instance.class = cls
            instance:ctor(...)
            return instance
        end
 
    else
        --从lua的类型中继承
        if super then
            cls = clone(super)
            cls.super = super
        else
       	--直接就没有super，那么定义cls为一个带有空ctor的table
            cls = {ctor = function() end}
        end
 
        cls.__cname = classname
        cls.__ctype = 2 -- lua
        cls.__index = cls
 
        --这里的new方法，首先是创建了空的table，然后将其metatable指向为cls
        --然后在调用ctor构造方法
        function cls.new(...)
            local instance = setmetatable({}, cls)
            instance.class = cls
            instance:ctor(...)
            return instance
        end
    end
    --返回类型的定义
    return cls
end