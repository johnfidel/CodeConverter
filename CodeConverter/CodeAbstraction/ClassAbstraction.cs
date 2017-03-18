using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeConverter
{
  public enum eVisiblity { Visibility_private, Visibility_protected, Visibility_public };

  /// <summary>
  /// abstraction for a member
  /// </summary>
  public class cMember
  {
    public string InlineComment
    {
      get { return m_InlineComment; }
      set { m_InlineComment = value; }
    }
    private string m_InlineComment = "";

    public List<String> Comment
    {
      get { return m_Comment; }
      set { m_Comment = value; }
    }
    private List<String> m_Comment = new List<string>();

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

    public override string ToString()
    {      
      return Enum.GetName(typeof(eVisiblity), m_Visibility).Replace("Visibility_", "") + " " + 
                          m_DataType + " " + Name;      
    }
  }
    
  /// <summary>
  /// Abstraction class for a method
  /// </summary>
  public class cMethod
  {
    public string InlineComment
    {
      get { return m_InlineComment; }
      set { m_InlineComment = value; }
    }
    private string m_InlineComment = "";

    public List<String> Comment
    {
      get { return m_Comment; }
      set { m_Comment = value; }
    }
    private List<String> m_Comment = new List<string>();

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

    public override string ToString()
    {
      if (m_Parameters.Count > 0)
      {
        return Enum.GetName(typeof(eVisiblity), m_Visibility).Replace("Visibility_", "") + " " +
              m_DataType + " " +
              Name + "(" + m_Parameters.Aggregate(new StringBuilder(), (builder, param) => 
                                                          builder.Append(param.ToString()).Append(", ")) + ")";
      }
      else
      {
        return Enum.GetName(typeof(eVisiblity), m_Visibility).Replace("Visibility_", "") + " " +
              m_DataType + " " +
              Name + "()";
      }
      
    }

  }

  /// <summary>
  /// abstraction for a class
  /// </summary>
  public class cClassAbstraction
  {
    public List<String> Comment
    {
      get { return m_Comment; }
      set { m_Comment = value; }
    }
    private List<String> m_Comment = new List<string>();

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
