#ifndef __SGF_LOG_HPP__
#define __SGF_LOG_HPP__ 1
#include <iostream>
#include <stdio.h>
#include <sgame/macros.hpp>
namespace SGF {
     void Log(const char* FILE,int LINE,const char* FUNC,const char * fmt,...);
     void LogError(const char* FILE,int LINE,const char* FUNC,const char * fmt,...);
     void LogDebug(const char* FILE,int LINE,const char* FUNC,const char * fmt,...);
}
#define LOG_ERROR(fmt, ...) printf(fmt,##__VA_ARGS__)
#define LOG_DEBUG(fmt, ...) printf(fmt,##__VA_ARGS__)
#define LOG_RELEASE(fmt, ...) SGF::Log(__FILE__ , __LINE__ , __FUNCTION__ ,fmt,##__VA_ARGS__)//SGF::Log(__FILE__.__LINE__,__FUNCTION__,fmt,##__VA_ARGS__)
#endif