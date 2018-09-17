--- SGF lua api
local function initEnv()
    local game = SGFGame; -- SGFGame Resigter in Game.cs
    --- 资源ROOT
    local resRoot = game.ResRoot;
    local src = CS.SGF.Core.Path.Legalization(resRoot .. "/src");
    local res = CS.SGF.Core.Path.Legalization(resRoot .. "/res");

    --- 添加Lua搜索目录
    -- game.Lua.AddLuaSearchPath(resRoot);
    game.Lua:AddLuaSearchPath(src);
    game.Lua:AddLuaSearchPath(res);
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
    SGF.WinMgr.Open("Window.MainWin");
end
main();
-- local ret,err =  pcall(main);
-- if(err) then
--     CS.UnityEngine.Debug.Log(err)
-- end
