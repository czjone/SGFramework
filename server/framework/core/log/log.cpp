#include <iostream>
#include <cstring>
#include <stdio.h>
#include <stdarg.h>
#include <execinfo.h>
#include <stdlib.h>

#include <sgame/log/log.hpp>

using namespace std;
static const int MAX_LOG_LENGTH = 16 * 1024;

namespace SGF
{
enum LogType
{
    LOGTYPE_DEBUG = 1,
    LOGTYPE_ERROR = 2,
    LOGTYPE_RELEASE = 3,
};
}

void __SGF_print_stacktrace()
{
    const int size = 16;
    void *array[size];
    int stack_num = backtrace(array, size);
    char **stacktrace = backtrace_symbols(array, stack_num);
    for (int i = 0; i < stack_num; ++i)
    {
        std::cout << stacktrace[i] << std::endl;
    }
    free(stacktrace);
}

void _log(SGF::LogType type, const char *format, va_list args)
{
    int bufferSize = MAX_LOG_LENGTH;
    char *buf = nullptr;
    do
    {
        buf = new (std::nothrow) char[bufferSize];
        if (buf == nullptr)
            return;
        int ret = vsnprintf(buf, bufferSize - 3, format, args);
        if (ret < 0)
        {
            bufferSize *= 2;
            delete[] buf;
        }
        else
            break;

    } while (true);

    switch (type)
    {
#if DEBUG
    case SGF::LogType::LOGTYPE_DEBUG:
        std::cout << "[ DEBUG ] " << buf << std::endl;
        break;
#endif
    case SGF::LogType::LOGTYPE_ERROR:
        std::cout << "[ ERROR ] " << buf << std::endl;
        __SGF_print_stacktrace();
        break;

    case SGF::LogType::LOGTYPE_RELEASE:
        std::cout << "[ INFO ] " << buf << std::endl;
        break;
    default:
        break;
    }
}

void print_error_stacktrace()
{
    int size = 16;
    void *array[16];
    int stack_num = backtrace(array, size);
    char **stacktrace = backtrace_symbols(array, stack_num);
    for (int i = 0; i < stack_num; ++i)
    {
        std::cout << stacktrace[i] << std::endl;
    }
    free(stacktrace);
}

void SGF::Log(const char *FILE, int LINE, const char *FUNC, const char *fmt, ...)
{
    va_list args;
    va_start(args, fmt);
    _log(SGF::LogType::LOGTYPE_RELEASE, fmt, args);
    va_end(args);
}

void SGF::LogError(const char *FILE, int LINE, const char *FUNC, const char *fmt, ...)
{
    va_list args;
    va_start(args, fmt);
    _log(LOGTYPE_ERROR, fmt, args);
    va_end(args);
}

void SGF::LogDebug(const char *FILE, int LINE, const char *FUNC, const char *fmt, ...)
{
    va_list args;
    va_start(args, fmt);
    _log(LOGTYPE_DEBUG, fmt, args);
    va_end(args);
}