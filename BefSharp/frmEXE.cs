using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BefSharp
{
    public partial class frmEXE : Form
    {
        private string EXEname, code;

        public frmEXE(string EXE,string Code)
        {
            code = Code;
            EXEname = EXE;
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = ((Button)sender).DialogResult;
            InlineCode.CompilerSettings CS = new InlineCode.CompilerSettings();
            CS.allowSave = cbSave.Checked;
            CS.exitEnd = cbClose.Checked;
            CS.runInstantly = cbAutorun.Checked;
            InlineCode.storeInlineCode(EXEname, code, CS);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = ((Button)sender).DialogResult;
            Close();
        }
    }
}
