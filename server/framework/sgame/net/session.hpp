#ifndef __SGF_NET_SESSION_HPP
#define __SGF_NET_SESSION_HPP 1

#include <sgame/macros.hpp>

namespace SGF_NS
{
namespace net
{
class session
{
public:
  virtual ~session();

  virtual size_t write(const char *bytes, size_t size) = 0;

  virtual size_t read(char *bytes, size_t size) = 0;

  virtual void close() = 0;

  virtual void flush() = 0;
};
} // namespace net

} // namespace SGF_NS

#endif