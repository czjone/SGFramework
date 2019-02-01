#ifndef __SGF_GAME_CONFING_HPP
#define __SGF_GAME_CONFING_HPP 1

#include <string>
#include <sgame/macros.hpp>

namespace SGF_NS
{
    namespace SGame
    {
        class config {
            public:
                std::string tcphost;
                short tcpport;

            public:
                static const config &Get();
        };
    } // SGame
    
} // SGF_NS


#endif