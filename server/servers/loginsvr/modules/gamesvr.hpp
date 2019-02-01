#ifndef __SGF_MODUELS_TCPSERVER_HPP
#define __SGF_MODUELS_TCPSERVER_HPP 1
#include <sgame/macros.hpp>
#include <sgame/modules.hpp>
#include <sgame/net/tcp.hpp>
#include <memory>

namespace SGF_NS
{
namespace SGame
{
class TcpServerModule : public Module
{
public:
  explicit TcpServerModule();
  explicit TcpServerModule(std::string host, int port);

  virtual ~TcpServerModule(void);

public:
  virtual Ret RunAsy() override;
  virtual bool IsAlive() const override;

private:
  std::shared_ptr<SGF_NS::net::TcpServer> mTcpPtr;
};
} // namespace SGame
} // namespace SGF_NS

#endif