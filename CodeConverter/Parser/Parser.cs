using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using CodeConverter.CodeAbstraction;
using CodeConverter.CodeAbstraction.Delphi;

namespace CodeConverter
{
  public abstract class cParser
  {
    public abstract cFileAbstraction parse(string filename);
  }   
}
