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
    public FileInfo Info
    {
      get { return m_Info; }
      set { m_Info = value; }
    }
    private FileInfo m_Info;

    public List<cClassAbstraction> Classes
    {
      get { return m_Classes; }
      set { m_Classes = value; }
    }
    private List<cClassAbstraction> m_Classes = new List<cClassAbstraction>();

  }
}
