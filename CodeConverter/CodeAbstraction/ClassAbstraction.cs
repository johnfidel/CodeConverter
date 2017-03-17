using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeConverter
{
  public class cMember
  {
    public string Name
    {
      get { return m_Name; }
      set { m_Name = value; }
    }
    private string m_Name = "";

    public string DataType
    {
      get { return m_DataType; }
      set { m_DataType = value; }
    }
    private string m_DataType = "";
  }
    
  public class cFunction
  {    
  }

  public class cClassAbstraction
  {
    public string ClassName
    {
      get { return m_ClassName; }
      set { m_ClassName = value; }
    }
    private string m_ClassName = "";

    public List<cMember> Members
    {
      get { return m_Members; }
      set { m_Members = value; }
    }
    private List<cMember> m_Members = new List<cMember>();

    public List<cFunction> Functions
    {
      get { return m_Functions; }
      set { m_Functions = value; }
    }
    private List<cFunction> m_Functions = new List<cFunction>();
  }
}
