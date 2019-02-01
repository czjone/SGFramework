#ifndef __SGF_CORE_MODULES_HPP
#define __SGF_CORE_MODULES_HPP 1

#include <iostream>
#include <memory>
#include <vector>
#include <sgame/macros.hpp>

namespace SGF_NS
{

class Module
{
protected:
  Module();
  virtual ~Module(void);

public:
  virtual Ret RunAsy() = 0;
  virtual bool IsAlive() const = 0;
};

typedef std::initializer_list<std::shared_ptr<Module>> Modules_list;

class Modules
{
public:
  Modules();
  virtual ~Modules();
  Ret RunWith(Modules_list modules);
  bool HasAlived() const;

  private:
    Ret StartAllModules();
    void WaitComplate();

private:
  Modules_list mModeList;
};
} // namespace SGF_NS

#endif