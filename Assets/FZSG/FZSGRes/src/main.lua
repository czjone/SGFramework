function print( ... )
    arg = { ... } ;
    local val =  "";
    for k,v in pairs(arg) do  val = val .. v ..","; end
    CS.UnityEngine.Debug.Log(val);
end

function main() 
    print( "ssssssssssssssssssssss" );
    local i = 1 / 0;
    print("dddddddddddddddddddddddd");
    print("dddddddddddddddddddddddd ccc");

    error("mmmmmmmmmmmmmmmmmmmmm");

    print(debug.traceback());
end

main();
-- xpcall(main, function() print(debug.traceback()) end);