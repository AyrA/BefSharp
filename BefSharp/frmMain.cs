using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BefSharp
{
    public partial class frmMain : Form
    {
        private BefungeCode BC;
        private frmExec ExecForm;
        private bool cont = true;
        private string fName;
        private bool changed;

        public frmMain()
        {
            InitializeComponent();
            tbCode.Font = EmbedFont.GetSpecialFont(11f);
            tbCode.Text = BefungeCode.FormatCode(tbCode.Text);
            tbCode.Select(0, 0);
            changed = false;

            //Not implemented
            settingsToolStripMenuItem.Visible = false;

            if (InlineCode.hasInlineCode())
            {
                newToolStripMenuItem.Dispose();
                saveToolStripMenuItem.Dispose();
                openToolStripMenuItem.Dispose();
                compileToolStripMenuItem.Text = "Reset";
                if (InlineCode.getSettings().allowSave)
                {
                    saveAsToolStripMenuItem.Text = "Save original source";
                }
                else
                {
                    saveAsToolStripMenuItem.Dispose();
                }
                tbCode.Text = InlineCode.getInlineCode();
                //Disables compilation
                SFD.Filter = "Befunge files 80x25|*.bf|Befunge files (minimum)|*.bf";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool Save()
        {
            if (!string.IsNullOrEmpty(fName))
            {
                try
                {
                    File.WriteAllText(SFD.FileName, BefungeCode.FormatCode(tbCode.Text));
                    return true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Cannot save. Please check the error below:\r\n" + ex.Message, "Error saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            else
            {
                return SaveAs();
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunCode();
        }

        private void RunCode()
        {
            BefungeCode.Actions A = BefungeCode.Actions.None;
            cont = true;
            if (BC == null)
            {
                Compile();
            }
            int StepCount = 0;
            while (cont && A != BefungeCode.Actions.Terminated)
            {
                StepCount++;
                switch (A = BC.Clock())
                {
                    case BefungeCode.Actions.InputChar:
                        if (tbInput.Text.Length > 0)
                        {
                            BC.Clock(tbInput.Text[0]);
                            tbInput.Text = tbInput.Text.Substring(1);
                            if (tbInput.Text.Length == 0)
                            {
                                tbInput.Text = "\n";
                            }
                        }
                        else
                        {
                            cont = false;
                        }
                        break;
                   case BefungeCode.Actions.InputInteger:
                        int i = 0;
                        if (int.TryParse(tbInput.Text, out i))
                        {
                            BC.Clock(tbInput.Text[0]);
                            tbInput.Text = string.Empty;
                        }
                        else
                        {
                            cont = false;
                        }
                        break;
                }
                if (StepCount > 2000 || A != BefungeCode.Actions.None)
                {
                    StepCount = 0;
                    tbCode.Select(BC.CurrentInstruction.Y * BefungeCode.W + BC.CurrentInstruction.X + BC.CurrentInstruction.Y * 2, 1);
                    ExecForm.update();
                    Application.DoEvents();
                }
            }
            if (A == BefungeCode.Actions.Terminated && InlineCode.hasInlineCode() && InlineCode.getSettings().exitEnd)
            {
                changed = false;
                Close();
            }
            ExecForm.update();
        }

        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BC == null)
            {
                Compile();
            }
            switch (BC.Clock())
            {
                case BefungeCode.Actions.InputChar:
                    if (tbInput.Text.Length > 0)
                    {
                        BC.Clock(tbInput.Text[0]);
                        tbInput.Text = tbInput.Text.Substring(1);
                        if (tbInput.Text.Length == 0)
                        {
                            tbInput.Text = "\n";
                        }
                    }
                    else
                    {
                        cont = false;
                    }
                    break;
                case BefungeCode.Actions.InputInteger:
                    int i = 0;
                    if (int.TryParse(tbInput.Text, out i))
                    {
                        BC.Clock(tbInput.Text[0]);
                        tbInput.Text = string.Empty;
                    }
                    else
                    {
                        cont = false;
                    }
                    break;
            }
            tbCode.Select(BC.CurrentInstruction.Y * BefungeCode.W + BC.CurrentInstruction.X + BC.CurrentInstruction.Y * 2, 1);
            ExecForm.update();
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InlineCode.hasInlineCode())
            {
                tbCode.Text = InlineCode.getInlineCode();
            }
            Compile();
        }

        private void tbCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                tbCode.SelectAll();
            }
        }

        private void Compile()
        {
            if (!string.IsNullOrEmpty(tbCode.Text))
            {
                if (ExecForm != null)
                {
                    ExecForm.Dispose();
                }
                tbCode.Text = BefungeCode.FormatCode(tbCode.Text);
                tbCode.SelectionStart = 0;
                tbCode.SelectionLength = 1;
                BC = new BefungeCode(tbCode.Text);
                BC.BefungeOutput += new BefungeOutputHandler(BC_BefungeOutput);
                ExecForm = new frmExec(BC);
                ExecForm.Show();
                this.BringToFront();
                this.Focus();
                lineConsole();
            }
        }

        private void BC_BefungeOutput(object value, BefungeCode.OutputType vt)
        {
            if(vt==BefungeCode.OutputType.Code)
            {
                tbCode.Text = BefungeCode.FormatCode(value.ToString());
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                cont = false;
            }
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = e.Handled = true;
                RunCode();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cont = false;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            cont = false;
            if (changed && !InlineCode.hasInlineCode())
            {
                switch (MessageBox.Show("Save changes?", "unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        if (!Save())
                        {
                            e.Cancel = true;
                            return;
                        }
                        break;
                    case DialogResult.No:
                        changed = false;
                        Close();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
            if (ExecForm != null)
            {
                ExecForm.Close();
                ExecForm.Dispose();
            }
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            lineConsole();
        }

        private void lineConsole()
        {
            if (ExecForm != null)
            {
                ExecForm.Location = new Point(this.Left + this.Width, this.Top);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private bool SaveAs()
        {
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                switch (SFD.FilterIndex)
                {
                    case 1: //80x25
                        try
                        {
                            File.WriteAllText(SFD.FileName, BefungeCode.FormatCode(tbCode.Text));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Cannot save. Please check the error below:\r\n" + ex.Message, "Error saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        fName = SFD.FileName;
                        break;
                    case 2: //compressed
                        try
                        {
                            File.WriteAllText(SFD.FileName, Minimalize(tbCode.Lines));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Cannot save. Please check the error below:\r\n" + ex.Message, "Error saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        fName = SFD.FileName;
                        break;
                    case 3: //exe
                        new frmEXE(SFD.FileName, tbCode.Text).ShowDialog();
                        break;
                }
                return true;
            }
            return false;
        }

        private string Minimalize(string[] p)
        {
            string output = string.Empty;
            foreach (string line in p)
            {
                //Check if there is content at all and add it
                if (line.Trim().Length > 0)
                {
                    output += line.TrimEnd() + "\r\n";
                }
            }
            //Removes last linebreak
            return output.TrimEnd();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void tbCode_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (changed)
            {

            }
            else
            {
                fName = null;
                tbCode.Text = string.Empty;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (InlineCode.getSettings().runInstantly)
            {
                this.BeginInvoke((MethodInvoker)RunCode);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                switch (MessageBox.Show("Save changes?", "unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        if (!Save())
                        {
                            return;
                        }
                        break;
                    case DialogResult.No:
                        changed = false;
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            if (OFD.ShowDialog()==DialogResult.OK)
            {
                tbCode.Text = File.ReadAllText(OFD.FileName);
            }
        }
    }
}
