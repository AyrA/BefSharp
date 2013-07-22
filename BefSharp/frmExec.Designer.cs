namespace BefSharp
{
    partial class frmExec
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.lbStack = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // tbConsole
            // 
            this.tbConsole.BackColor = System.Drawing.Color.Black;
            this.tbConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbConsole.ForeColor = System.Drawing.Color.Gray;
            this.tbConsole.Location = new System.Drawing.Point(0, 0);
            this.tbConsole.Multiline = true;
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.ReadOnly = true;
            this.tbConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbConsole.Size = new System.Drawing.Size(759, 434);
            this.tbConsole.TabIndex = 0;
            this.tbConsole.Text = "Console\r\n";
            // 
            // lbStack
            // 
            this.lbStack.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbStack.FormattingEnabled = true;
            this.lbStack.Location = new System.Drawing.Point(759, 0);
            this.lbStack.Name = "lbStack";
            this.lbStack.Size = new System.Drawing.Size(120, 433);
            this.lbStack.TabIndex = 1;
            // 
            // frmExec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 434);
            this.Controls.Add(this.tbConsole);
            this.Controls.Add(this.lbStack);
            this.Name = "frmExec";
            this.Text = "Befunge Execution";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExec_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbConsole;
        private System.Windows.Forms.ListBox lbStack;
    }
}