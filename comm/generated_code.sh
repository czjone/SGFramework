rm -rf build
mkdir -p build
mkdir -p build/csharp
mkdir -p build/java
mkdir -p build/cpp
mkdir -p build/lua
./flatc -o build/cpp -c --gen-object-api ./SGFComm.fbs ./configs.fbs
./flatc -o build/java -j ./SGFComm.fbs ./configs.fbs
./flatc -o build/lua -l ./SGFComm.fbs ./configs.fbs
./flatc -o build/csharp -n ./SGFComm.fbs ./configs.fbs
