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
    private enum eVisiblity { Visibility_private, Visibility_protected, Visibility_public };

    private void parseStatement(List<string> statement, eVisiblity visiblity, cClassAbstraction classRef)
    {
      switch (visiblity)
      {
        case eVisiblity.Visibility_private:
        {

            break;
        }
      }

      }
    }

    private List<string> parseToEscapeCharacter(List<string> content, int ArrayPos)
    {
      List<string> block = new List<string>();
      for (int i = ArrayPos; i < content.Count; i++)
      {
        if ((content[i] == ";") || (content[i][content[i].Length-1] == ';'))
        {
          block.Add(content[i]);
          break;
        }
        block.Add(content[i]);
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

            List<string> block = parseToEscapeCharacter(content, ArrayPos);
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

      // retrieve all words in file separately
      string[] words = unescapedblock.Split(' ').Where(word => word != "").ToArray();

      // let the parser work
      return parseDelphi(words);
    }
  }
}
