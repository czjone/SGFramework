#ifndef __SGF_NET_TCP_HPP
#define __SGF_NET_TCP_HPP 1

#include <string>
#include <sgame/macros.hpp>
#include <sgame/net/session.hpp>

namespace SGF_NS
{
namespace net
{
enum TcpServerState
{
    INIT,
    RUNNING,
    PAUSE,
    STOP,
};

class TcpServer
{
  public:
    TcpServer(short port);
    TcpServer(std::string host, short port);
    virtual ~TcpServer();

  public:
    Ret StartAsy();
    void Pause();
    void Resume();
    void Stop();
    void Bind();

    TcpServerState GetState() const;

  protected:
    void Accept();

  private:
    TcpServerState mstate;
    void *mAcceptor;
    void *mIOContext;
    void *mEndPoint;
};
} // namespace net

} // namespace SGF_NS

#endif