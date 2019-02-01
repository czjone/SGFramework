#include <boost/asio.hpp>
#include <boost/thread/thread.hpp>

#include <sgame/log/log.hpp>
#include <sgame/net/tcp.hpp>
#include <sgame/macros.hpp>

using SGF_NS::net::TcpServer;
using SGF_NS::net::TcpServerState;

using boost::asio::io_context;
using boost::asio::ip::tcp;

using boost::thread;

TcpServer::TcpServer(short port)
{
    new (this) TcpServer("127.0.0.1", port);
}

TcpServer::TcpServer(std::string host, short port)
    : mIOContext(new boost::asio::io_context())
{
    LOG_RELEASE("tcp server:%s:%hd", host.c_str(), port);

    boost::asio::ip::address add;
    add.from_string(host);
    mEndPoint = new tcp::endpoint(add, port);

    auto iocontext = (io_context *)(this->mIOContext);
    auto endpoint = (tcp::endpoint *)(this->mEndPoint);
    mAcceptor = new tcp::acceptor(*iocontext, *endpoint);
}

TcpServer::~TcpServer()
{
    SAFE_DELETE(mAcceptor);
    SAFE_DELETE(mIOContext);
    SAFE_DELETE(mEndPoint);
}

void TcpServer::Accept()
{
    LOG_DEBUG("wait new connect...");
    tcp::acceptor *acceptor_ = (tcp::acceptor *)(this->mAcceptor);
    acceptor_->async_accept(
        [this](boost::system::error_code ec, tcp::socket socket) {
            if (!ec)
            {
                // std::make_shared<chat_session>(std::move(socket), room_)->start();
            }
            this->Accept();
        });
}

Ret TcpServer::StartAsy()
{
    auto *io_ctx = (boost::asio::io_context *)(this->mIOContext);
    io_ctx->poll_one();
}

void TcpServer::Pause()
{
    this->mstate = TcpServerState::PAUSE;
    auto *io_ctx = (boost::asio::io_context *)(this->mIOContext);
    io_ctx->stop();
    io_ctx->reset();
}

void TcpServer::Resume()
{
    this->mstate = TcpServerState::RUNNING;
    this->StartAsy();
}

void TcpServer::Stop()
{
    this->mstate = TcpServerState::STOP;
    auto *io_ctx = (boost::asio::io_context *)(this->mIOContext);
    io_ctx->stop();
}

TcpServerState TcpServer::GetState() const
{
    return this->mstate;
}
