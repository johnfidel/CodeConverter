using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeConverter.CodeAbstraction
{
  public class cFileAbstraction
  {
    public string FileName
    {
      get { return m_FileName; }
      set { m_FileName = value; }
    }
    private string m_FileName;

    public List<cClassAbstraction> Classes
    {
      get { return m_Classes; }
      set { m_Classes = value; }
    }
    private List<cClassAbstraction> m_Classes = new List<cClassAbstraction>();
  }
}
