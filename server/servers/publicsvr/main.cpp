#include <memory>
#include <sgame/macros.hpp>
#include <sgame/modules.hpp>
#include "modules/gamesvr.hpp"

using SGF_NS::Module;
using SGF_NS::Modules;
using SGF_NS::SGame::TcpServerModule ;
using std::unique_ptr;

extern "C"
{
   int main(int c, char **args)
   {
      unique_ptr<Modules> modules(new Modules);
      return modules->RunWith({ std::shared_ptr<Module>(new TcpServerModule())});
   }
}