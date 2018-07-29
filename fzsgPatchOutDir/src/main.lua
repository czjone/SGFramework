require("core.init")

function main() 
    SGF.WinMgr.open("wins.loginWin");
end
main();
-- xpcall(main, function() print(debug.traceback()) end);