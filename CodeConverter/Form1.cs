using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CodeConverter.Translater;
using CodeConverter.Translater.Delphi;
using CodeConverter.CodeAbstraction;
using CodeConverter.Parser;
using CodeConverter.Parser.Delphi;

namespace CodeConverter
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void btnRun_Click(object sender, EventArgs e)
    {
      cParser parser = new cDelphiFileParser();
      cFileAbstraction file = parser.parse(txtFile.Text);

      // translate into selected language
      cTranslater translator = new cDelphiCodeTranslater();
      translator.translate(file);
    }
  }
}
