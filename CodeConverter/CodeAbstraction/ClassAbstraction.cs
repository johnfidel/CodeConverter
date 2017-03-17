using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeConverter
{
  public enum eVisiblity { Visibility_private, Visibility_protected, Visibility_public };

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

    public eVisiblity Visibility
    {
      get { return m_Visibility; }
      set { m_Visibility = value; }
    }
    private eVisiblity m_Visibility = eVisiblity.Visibility_private;
  }
    
  public class cMethod
  {

    public string Name
    {
      get { return m_Name; }
      set { m_Name = value; }
    }
    private string m_Name = "";

    public List<cMember> Parameters
    {
      get { return m_Parameters; }
      set { m_Parameters = value; }
    }
    private List<cMember> m_Parameters = new List<cMember>();

    public string DataType
    {
      get { return m_DataType; }
      set { m_DataType = value; }
    }
    private string m_DataType = "";

    public eVisiblity Visibility
    {
      get { return m_Visibility; }
      set { m_Visibility = value; }
    }
    private eVisiblity m_Visibility = eVisiblity.Visibility_private;

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

    public List<cMethod> Methods
    {
      get { return m_Methods; }
      set { m_Methods = value; }
    }
    private List<cMethod> m_Methods = new List<cMethod>();
  }
}
