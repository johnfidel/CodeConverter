using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeConverter.CodeAbstraction;

namespace CodeConverter.Translater
{
  public abstract class cTranslater
  {
    public abstract string translate(cFileAbstraction file);
  }
}
