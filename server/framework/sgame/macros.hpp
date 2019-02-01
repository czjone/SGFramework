#ifndef __SGF_MACROS_HPP
#define __SGF_MACROS_HPP 1

#define SGF_NS SGF

#define Ret short
#define SUCCESS 0
#define Fail 1

#define SAFE_DELETE(P)    \
    do                    \
    {                     \
        if (P != nullptr) \
        {                 \
            delete P;     \
            P == nullptr; \
        }                 \
    } while (0)

#define SAFE_DELETE_ARRAY(P) \
    do                       \
    {                        \
        if (P != nullptr)    \
        {                    \
            delete[] P;      \
            P == nullptr;    \
        }                    \
    } while (0)

#endif