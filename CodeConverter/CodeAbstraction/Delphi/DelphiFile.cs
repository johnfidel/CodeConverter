using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConverter.CodeAbstraction.Delphi
{
  public class cDelphiFile : cFileAbstraction
  {
    public List<string> InterfaceSection
    {
      get { return m_InterfaceSection; }
      set { m_InterfaceSection = value; }
    }
    private List<string> m_InterfaceSection = new List<string>();

    public List<string> ImplementationSection
    {
      get { return m_ImplementationSection; }
      set { m_ImplementationSection = value; }
    }
    private List<string> m_ImplementationSection = new List<string>();
  }
}
