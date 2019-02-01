#include "servers/gamesvr/modules/config.hpp"
// #include "flatbuffers/flatbuffers.h"
#include <map>
#include <string>
#include <vector>

using SGF_NS::SGame::config;
using std::map;
using std::string;

map<string,void*> __s_configs_cache;

const config &config::Get()
{
    const char *fname = "config";
    
    if(__s_configs_cache.find(fname) != __s_configs_cache.end()){

    }
}