function traceback( msg )
    print("----------------------------------------")
    print("LUA ERROR: " .. tostring(msg) .. "\n")
    print(debug.traceback())
    print("----------------------------------------")
end
local function main()
    print("hello")
end
xpcall(main, traceback)