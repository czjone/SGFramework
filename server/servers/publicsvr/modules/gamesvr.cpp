#include <sgame/net/tcp.hpp>
#include "servers/gamesvr/modules/gamesvr.hpp"
#include "servers/gamesvr/modules/config.hpp"

using SGF_NS::net::TcpServer;
using SGF_NS::net::TcpServerState;
using SGF_NS::SGame::config;
using SGF_NS::SGame::TcpServerModule;

TcpServerModule::TcpServerModule()
{
    auto &conf = config::Get();
    new (this) TcpServerModule(conf.tcphost, conf.tcpport);
}

TcpServerModule::TcpServerModule(std::string host, int port)
    : mTcpPtr(new TcpServer(host, port))
{
}

TcpServerModule::~TcpServerModule(void)
{
}

Ret TcpServerModule::RunAsy()
{
    return this->mTcpPtr->StartAsy();
}

bool TcpServerModule::IsAlive() const
{
    return this->mTcpPtr->GetState() != TcpServerState::STOP;
}