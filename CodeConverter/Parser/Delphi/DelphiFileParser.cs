using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using CodeConverter.CodeAbstraction;
using CodeConverter.CodeAbstraction.Delphi;

namespace CodeConverter.Parser.Delphi
{
  public class cDelphiFileParser : cParser
  {    
    private enum eStatementType { StatementType_unknown, StatementType_function, StatementType_procedure, StatementType_member };

    /// <summary>
    /// removes visibility
    /// </summary>
    /// <param name="statement"></param>
    private void removeVisibility(List<string> statement)
    {
      if (statement.Count > 0)
      {
        if ((statement[0] == "private") ||
            (statement[0] == "protected") ||
            (statement[0] == "public"))
        {
          statement.RemoveAt(0);
        }
      }      
    }

    /// <summary>
    /// checks the type of statement
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private eStatementType getStatementType(List<string> statement)
    {
      if (statement.Count > 0)
      {
        if (statement[0] == "procedure")
        {
          return eStatementType.StatementType_procedure;
        }
        else if (statement[0] == "function")
        {
          return eStatementType.StatementType_function;
        }
        else if (statement.Count >= 2)
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
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private cMember parseMember(List<string> statement)
    {
      cMember member = new cMember();
      if (statement.Count > 2)
      {
        member.Name = statement[0];
        member.DataType = statement[2].Replace(';', ' ').Trim();
      }
      else
      {
        member.Name = statement[0].Replace(':', ' ').Trim();
        member.DataType = statement[1].Replace(';', ' ').Trim();
      }
      return member;
    }

    /// <summary>
    /// parse for a method
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private cMethod parseFunction(List<string> statement)
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
        List<string> returnValue = parseToEscapeCharacter(statement, ref ArrayPos);
        if (returnValue.Last() != ";")
        {
          method.DataType = returnValue.Last().Replace(';', ' ').Trim();
        }
        else
        {
          method.DataType = returnValue[returnValue.Count-2];
        }        
      }
      else
      {
        if (statement.Last() != ";")
        {
          method.DataType = statement.Last().Replace(';', ' ').Trim();
        }
        else
        {
          method.DataType = statement[statement.Count - 2];
        }
      }


      return method;
    }

    /// <summary>
    /// parse for a method
    /// </summary>
    /// <param name="statement"></param>
    /// <returns></returns>
    private cMethod parseProcedure(List<string> statement)
    {
      cMethod method = new cMethod();

      method.Name = statement[1];

      return method;
    }

    /// <summary>
    /// create statement and add it to the class definition
    /// </summary>
    /// <param name="statement"></param>
    /// <param name="visiblity"></param>
    /// <param name="classRef"></param>
    private void parseStatement(List<string> statement, eVisiblity visiblity, cClassAbstraction classRef)
    {
      eStatementType type;
         
      // remove visibility if available
      removeVisibility(statement);

      // then check wheter if it is a method or a member    
      type = getStatementType(statement);

      switch (type)
      {
        case eStatementType.StatementType_member:
          {
            cMember member = parseMember(statement);
            classRef.Members.Add(member);
            break;
          }

        case eStatementType.StatementType_procedure:
          {
            cMethod method = parseProcedure(statement);
            method.Visibility = visiblity;
            classRef.Methods.Add(method);
            break;
          }

        case eStatementType.StatementType_function:
          {
            cMethod method = parseFunction(statement);
            method.Visibility = visiblity;
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
        if (content[ArrayPos] == "(") { inQuote = true; }
        if (content[ArrayPos] == ")") { if (inQuote) { inQuote = false; } else { break; } }

        if (((content[ArrayPos] == ";") || (content[ArrayPos][content[ArrayPos].Length-1] == ';')) &&
          (!inQuote))
        {
          block.Add(content[ArrayPos]);
          break;
        }
        block.Add(content[ArrayPos]);
      }
      return block;
    }

    private cClassAbstraction parseInterface(List<string> content)
    {
      cClassAbstraction parsedClass = new cClassAbstraction();
      eVisiblity visibility = eVisiblity.Visibility_private;
      bool isClass = false;

      int ArrayPos;
      for (ArrayPos = 0; ArrayPos < content.Count; ArrayPos++)
      {
        string actualWord = content[ArrayPos].Trim();

        // class 
        if ((actualWord == "type") &&
            (content[ArrayPos + 2] == "=") &&
            (content[ArrayPos + 3] == "class"))
        {
          parsedClass.ClassName = content[ArrayPos + 1];
          ArrayPos += 3;
          isClass = true;
        }
        else
        {
          if (isClass)
          {
            // class members
            if (actualWord == "private") { visibility = eVisiblity.Visibility_private; }
            if (actualWord == "protected") { visibility = eVisiblity.Visibility_protected; }
            if (actualWord == "public") { visibility = eVisiblity.Visibility_public; }

            List<string> block = parseToEscapeCharacter(content, ref ArrayPos);
            parseStatement(block, visibility, parsedClass);
          }
        }
      }
      return parsedClass;
    }

    /// <summary>
    /// parse the delphi content of file. Split up to interface and implementation sections
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private cDelphiFile parseDelphi(string[] content)
    {
      cDelphiFile file = new cDelphiFile();

      // split up file sections
      bool isInterface = false;
      bool isImplementation = false;
      foreach (string word in content)
      {
        if (word == "interface") { isInterface = true; isImplementation = false; continue; }
        if (word == "implementation") { isInterface = false; isImplementation = true; continue; }

        if (isInterface)
        {
          file.InterfaceSection.Add(word);
        }

        if (isImplementation)
        {
          file.ImplementationSection.Add(word);
        }
      }

      file.Classes.Add(parseInterface(file.InterfaceSection));

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
      cDelphiFile file = new cDelphiFile();

      string tmp = reader.ReadToEnd();

      // remove escape sequences
      string unescapedblock = tmp.Aggregate(new StringBuilder(), (builder, character) =>
                                              escapedChar.Contains(character) ? builder.Append(' ') : builder.Append(character))
                                              .ToString();

      // make sure to split words wich have '(' or ')' inside
      string unescapedBrackletHandledblock = unescapedblock.Aggregate(new StringBuilder(), (builder, character) =>
                                                                ((character == '(') || (character == ')')) ? builder.Append(' ').Append(character).Append(' ') : builder.Append(character))
                                                                .ToString();

      // retrieve all words in file separately
      string[] words = unescapedBrackletHandledblock.Split(' ').Where(word => word != "").ToArray();

      // let the parser work
      return parseDelphi(words);
    }
  }
}
