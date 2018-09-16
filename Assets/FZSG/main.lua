--- SGF lua api
local function initEnv()
    --- 注册全局API,接口来自于RootBehaviour.cs的注册
    SGF = {
        RootBehaviour = RootBehaviour,
        UICamera = RootBehaviour.gameObject,
        Lua = RootBehaviour.Game.Lua,
        UIRoot = RootBehaviour.UIRoot,
    }
    --- 资源ROOT
    local resRoot = CS.SGF.Core.Path.Legalization(SGF.RootBehaviour.ResRoot.."/FZSGRes");
    --- 脚本目录
    local src = CS.SGF.Core.Path.Legalization(resRoot .. "/src");
    --- 资源目录
    local res = CS.SGF.Core.Path.Legalization(resRoot .. "/res");
    --- 添加Lua搜索目录
    SGF.Lua:AddLuaSearchPath(src);
    SGF.Lua:AddLuaSearchPath(res);
end

local function loadBaseLib()
    require("Base.Init");
end

local function appEnvConfig()
    setPrintPower(true) -- 是否启用打印日志
end

--- 程序入口
local function main()
    --- 初始化Lua运行环境
    initEnv();
    --- 加载基础库
    loadBaseLib();
    --- 应用一些常用的设置
    appEnvConfig();
    --- 打开主窗口
    SGF.WinMgr.open("Window.MainWin");
end
main();
-- local ret,err =  pcall(main);
-- if(err) then
--     CS.UnityEngine.Debug.Log(err)
-- end
