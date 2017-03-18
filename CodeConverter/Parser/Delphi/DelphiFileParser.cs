using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using CodeConverter.CodeAbstraction;
using CodeConverter.CodeAbstraction.Delphi;

namespace CodeConverter.Parser.Delphi
{
  public class cDelphiFileParser : cParser
  {    
    private enum eStatementType { StatementType_unknown, StatementType_function, StatementType_procedure, StatementType_member };

    /// <summary>
    /// transform a string list into a simple word array. splits
    /// up ';', ':', '(' and ')' single elements
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private List<string> transformToList(List<string> content)
    {
      char[] chars = new[] { ';', ':' };
      string alllines = content.Aggregate(new StringBuilder(), (builder, line) =>
                                                                  builder.Append(line)).ToString();

      string linewithescapedchars = alllines.Aggregate(new StringBuilder(), (builder, character) =>
                                                                  (chars.Contains(character) ? 
                                                                    builder.Append(' ').Append(character).Append(' ') : 
                                                                    builder.Append(character)))
                                                                  .ToString();

      string[] words = linewithescapedchars.Split(' ');
      // remove emty places
      return words.Where(word => word != "").ToList(); ;
    }

    /// <summary>
    /// removes visibility
    /// PRECONDITION: the statement must be a list of single words per item
    /// </summary>
    /// <param name="statement"></param>
    private bool getAndRemoveVisibility(List<string> statement, out eVisiblity visibility)
    {
      bool result = true;
      visibility = eVisiblity.Visibility_private;

      if (statement.Count > 0)
      {
        if (statement[0] == "private")
        {
          visibility = eVisiblity.Visibility_private;
          statement.RemoveAt(0);
        }
        else if (statement[0] == "protected")
        {
          visibility = eVisiblity.Visibility_protected;
          statement.RemoveAt(0);
        }
        else if (statement[0] == "public")
        {
          visibility = eVisiblity.Visibility_public;
          statement.RemoveAt(0);
        }
        else
        {
          result = false;
        }
      }
      else
      {
        result = false;
      }
      return result;
    }

    /// <summary>
    /// checks the type of statement
    /// PRECONDITION: the statement must be a list of single words per item
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private eStatementType getStatementType(List<string> statement)
    {
      if (statement.Count > 0)
      {
        if (statement[0].Contains("procedure"))
        {
          return eStatementType.StatementType_procedure;
        }
        else if (statement[0].Contains("function"))
        {
          return eStatementType.StatementType_function;
        }
        else if (statement[1].Contains(":"))
        {
          return eStatementType.StatementType_member;
        }
        else
        {
          return eStatementType.StatementType_unknown;
        }
      }
      return eStatementType.StatementType_unknown;
    }

    /// <summary>
    /// parse for a member
    /// PRECONDITION: the statement must be a list of single words per item
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private cMember parseMember(List<string> statement)
    {
      List<string> words = transformToList(statement);

      cMember member = new cMember();
      if (words.Count > 2)
      {
        member.Name = words[0];
        member.DataType = words[2];
      }      
      return member;
    }

    /// <summary>
    /// parse for a method
    /// PRECONDITION: the statement must be a list of single words per item
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private cMethod parseFunction(List<string> statement)
    {
      cMethod method = parseProcedure(statement);
      // get return value     
      method.DataType = statement[statement.Count - 2];

      return method;
    }

    /// <summary>
    /// parse for a method
    /// PRECONDITION: the statement must be a list of single words per item
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private cMethod parseProcedure(List<string> statement)
    {
      int ArrayPos = 0;
      cMethod method = new cMethod();
      method.Name = statement[1];

      // do we have parameters
      if (statement.Contains("("))
      {
        int PosOfBracklet = statement.FindIndex(word => word == ")");
        for (ArrayPos = 3; ArrayPos < PosOfBracklet; ArrayPos++)
        {
          List<string> parameter = parseToEscapeCharacter(statement, ref ArrayPos);
          cMember member = parseMember(parameter);
          method.Parameters.Add(member);
        }        
      }    
      return method;
    }

    /// <summary>
    /// create statement and add it to the class definition
    /// </summary>
    /// <param name="statement"></param>
    /// <param name="visiblity"></param>
    /// <param name="classRef"></param>
    private eVisiblity m_visibility = eVisiblity.Visibility_private;
    private void parseStatement(List<string> statement, cClassAbstraction classRef)
    {
      eStatementType type;      
         
      // distinct comment from real statement
      List<string> comment = statement.Where(line => line.Trim().First() == '/').ToList();
      // extract inline comment
      List<string> realstatement = new List<string>();
      List<string> inlinecomment = new List<string>();
      foreach (string line in statement.Where(line => line.Trim().First() != '/').ToList())
      {
        if (line.Contains("//"))
        {
          string[] substrings = line.Split('/');
          realstatement.Add(substrings.First());
          inlinecomment.Add(substrings.Last().Insert(0, "// "));
        }
        else
        {
          realstatement.Add(line);
        }
      }

      List<string> words = transformToList(realstatement);
      // then check wheter if it is a method or a member    
      eVisiblity newVisibility = eVisiblity.Visibility_private;
      if (getAndRemoveVisibility(words, out newVisibility)) { m_visibility = newVisibility; }
      type = getStatementType(words);

      switch (type)
      {
        case eStatementType.StatementType_member:
          {
            cMember member = parseMember(words);
            member.Visibility = m_visibility;
            member.Comment = comment;
            member.InlineComment += inlinecomment; 
            classRef.Members.Add(member);
            break;
          }

        case eStatementType.StatementType_procedure:
          {
            cMethod method = parseProcedure(words);
            method.Visibility = m_visibility;
            method.Comment = comment;
            method.InlineComment += inlinecomment;
            classRef.Methods.Add(method);
            break;
          }

        case eStatementType.StatementType_function:
          {
            cMethod method = parseFunction(words);
            method.Visibility = m_visibility;
            method.Comment = comment;
            method.InlineComment += inlinecomment;
            classRef.Methods.Add(method);
            break;
          }       
      }
    }

    /// <summary>
    /// get an entiry statement block until escape character ';'
    /// </summary>
    /// <param name="content"></param>
    /// <param name="ArrayPos"></param>
    /// <returns></returns>
    private List<string> parseToEscapeCharacter(List<string> content, ref int ArrayPos)
    {
      List<string> block = new List<string>();
      bool inQuote = false;
      for (; ArrayPos < content.Count; ArrayPos++)
      {
        if (content[ArrayPos].Contains("(")) { inQuote = true; }
        if (content[ArrayPos].Contains(")")) { if (inQuote) { inQuote = false; } else { break; } }

        if ((content[ArrayPos].Contains(";")) &&
          (!inQuote))
        {
          block.Add(content[ArrayPos]);
          break;
        }
        block.Add(content[ArrayPos]);
      }
      // remove emtpy lines
      return block.Where(line => line != "").ToList();
    }

    /// <summary>
    /// parse all classes in interface section
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private List<cClassAbstraction> parseInterface(List<string> content)
    {
      List<cClassAbstraction> parsedClasses = new List<cClassAbstraction>();
      cClassAbstraction actualClass = new cClassAbstraction();
                                                                            
      bool isClass = false;

      int ArrayPos;      
      for (ArrayPos = 0; ArrayPos < content.Count; ArrayPos++)
      {
        string actualLine = content[ArrayPos].Trim();
        if (actualLine != "end;")
        {
          string[] words = actualLine.Split(' ');     

          // class 
          if ((actualLine.Contains("=")) &&
              (actualLine.Contains("class")))
          {            
            actualClass.ClassName = actualLine.Replace("type", " ")
                                              .Replace("=", " ")
                                              .Replace("class", " ").Trim();            
            isClass = true;
          }
          else
          {
            if (isClass)
            {
              if (actualLine != "")
              {
                List<string> block = parseToEscapeCharacter(content, ref ArrayPos);                
                parseStatement(block, actualClass);
              }
            }
            else
            {
              // comment
              if ((words.First() == "//") || (words.First() == "///"))
              {
                actualClass.Comment.Add(actualLine + '\r' + '\n');
              }
            }
          }
        }
        else
        {
          isClass = false;
          parsedClasses.Add(actualClass);
          actualClass = new cClassAbstraction();
        }
      }
      return parsedClasses;
    }

    private enum eTextType { TextType_unknown, TextType_comment, TextType_unit, TextType_interface, TextType_implementation };

    /// <summary>
    /// parse the delphi content of file. Split up to interface and implementation sections
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private cDelphiFile parseDelphi(List<string> content)
    {
      eTextType texttype = eTextType.TextType_unknown;  
      cDelphiFile file = new cDelphiFile();

      // get all classes inside this file
      // split up file sections      
      foreach (string line in content)
      {
        if (line != "")
        {
          string[] words = line.Split(' ');

          if (words.Last() != "end.")
          {
            if ((words.First() == "//") || (words.First() == "///")) { texttype = eTextType.TextType_comment; }
            if (words.First() == "unit") { texttype = eTextType.TextType_unit; }
            if (words.First() == "interface") { texttype = eTextType.TextType_interface; }
            if (words.First() == "implementation") { texttype = eTextType.TextType_implementation; }

            switch (texttype)
            {
              case eTextType.TextType_comment:
                {
                  file.FileHeader += line + '\r' + '\n';
                  break;
                }

              case eTextType.TextType_unit:
                {
                  file.FileName = words.Last();
                  break;
                }

              case eTextType.TextType_interface:
                {
                  file.InterfaceSection.Add(line);
                  break;
                }

              case eTextType.TextType_implementation:
                {
                  file.ImplementationSection.Add(line);
                  break;
                }
            }
          }
          else
          {
            // first get interface of classes
            file.Classes.AddRange(parseInterface(file.InterfaceSection));
          }
        }
      }
      return file;
    }  

    /// <summary>
    /// interface method to class cParser
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public override cFileAbstraction parse(string filename)
    {
      StreamReader reader = new StreamReader(filename);
      char[] escapedChar = new[] { '\t', '\r', '\n' };

      cClassAbstraction parsedClass = new cClassAbstraction();

      List<string> lines = new List<string>();
      while (!reader.EndOfStream)
      {
        string line = reader.ReadLine();
        // remove escape sequences
        string unescapedblock = line.Aggregate(new StringBuilder(), (builder, character) =>
                                                escapedChar.Contains(character) ? builder.Append(' ') : builder.Append(character))
                                                .ToString();

        // make sure to split words wich have '(' or ')' inside
        string unescapedBrackletHandledblock = unescapedblock.Aggregate(new StringBuilder(), (builder, character) =>
                                                                  ((character == '(') || (character == ')')) ? builder.Append(' ').Append(character).Append(' ') : builder.Append(character))
                                                                  .ToString();

        lines.Add(unescapedBrackletHandledblock);
      }
      reader.Close();

      // let the parser work
      return parseDelphi(lines);
    }
  }
}
