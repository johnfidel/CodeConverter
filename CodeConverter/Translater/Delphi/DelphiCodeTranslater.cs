using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeConverter.CodeAbstraction;
using CodeConverter.CodeAbstraction.Delphi;
using CodeConverter.Translater;

namespace CodeConverter.Translater.Delphi
{
  public class cDelphiCodeTranslater : cTranslater
  {

    private List<string> parseForMethodBody(string name, List<string> content)
    {
      return new List<string>();
    }

    public override string translate(cFileAbstraction file)
    {
      cDelphiFile delphiFile = (cDelphiFile)file;
      foreach (cClassAbstraction c in file.Classes)
      {
        foreach (cMethod m in c.Methods)
        {
          
          List<string> body = parseForMethodBody(c.ClassName + "." + m.Name, delphiFile.ImplementationSection);
        }        
      }
      return "";
    }
  }
}

