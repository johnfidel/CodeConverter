namespace CodeConverter
{
  partial class Form1
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.txtFile = new System.Windows.Forms.TextBox();
      this.btnRun = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // txtFile
      // 
      this.txtFile.Location = new System.Drawing.Point(12, 12);
      this.txtFile.Name = "txtFile";
      this.txtFile.Size = new System.Drawing.Size(375, 20);
      this.txtFile.TabIndex = 0;
      this.txtFile.Text = "\\\\VBOXSVR\\rappic\\workspace\\CodeConverter\\ExampleCode\\UTest.pas";
      // 
      // btnRun
      // 
      this.btnRun.Location = new System.Drawing.Point(312, 38);
      this.btnRun.Name = "btnRun";
      this.btnRun.Size = new System.Drawing.Size(75, 23);
      this.btnRun.TabIndex = 1;
      this.btnRun.Text = "Run";
      this.btnRun.UseVisualStyleBackColor = true;
      this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(399, 171);
      this.Controls.Add(this.btnRun);
      this.Controls.Add(this.txtFile);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtFile;
    private System.Windows.Forms.Button btnRun;
  }
}

