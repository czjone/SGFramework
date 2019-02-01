#include <thread>
#include <boost/thread/thread.hpp>

#include <sgame/modules.hpp>
#include <sgame/log/log.hpp>

using namespace SGF_NS;

////////////////////////////////////////////////////////
Module::Module()
{
}

Module::~Module(void)
{
}

////////////////////////////////////////////////////////

Modules::Modules()
{
}
Modules::~Modules()
{
}

Ret Modules::RunWith(const Modules_list modules)
{
    this->mModeList = modules;
    this->StartAllModules();
    this->WaitComplate();
    return SUCCESS;
}

Ret Modules::StartAllModules()
{
    for (auto &module : this->mModeList)
    {
        module->RunAsy();
    }
}

void Modules::WaitComplate()
{
    //block main thread.
    while (this->HasAlived())
    {
        boost::this_thread::sleep(boost::posix_time::seconds(2));
    }
}

bool Modules::HasAlived() const
{
    for (auto &itr : this->mModeList)
    {
        if (itr->IsAlive())
        {
            return true;
        }
    }
    return false;
}