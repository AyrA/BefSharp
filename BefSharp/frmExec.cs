using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BefSharp
{
    public partial class frmExec : Form
    {
        private BefungeCode BC;
        private BefungeOutputHandler Bout;
        private BefungeStackChangedHandler Bstack;
        private string console;
        private Stack<int> BfStack;

        public frmExec(BefungeCode BefC)
        {
            InitializeComponent();
            BC = BefC;
            Bout=new BefungeOutputHandler(BC_BefungeOutput);
            Bstack = new BefungeStackChangedHandler(BC_BefungeStackChanged);
            BC.BefungeOutput += Bout;
            BC.BefungeStackChanged += Bstack;
            tbConsole.Font = EmbedFont.GetSpecialFont(11);
            BfStack = new Stack<int>();
            console = string.Empty;
        }

        public void update()
        {
            if (!string.IsNullOrEmpty(console))
            {
                tbConsole.Text = console;
                tbConsole.Select(tbConsole.Text.Length - 1, 1);
                tbConsole.ScrollToCaret();
            }
            lbStack.Items.Clear();
            if (BfStack.Count > 0)
            {
                int[] items = new int[BfStack.Count];
                BfStack.CopyTo(items, 0);
                foreach (int ii in items)
                {
                    lbStack.Items.Insert(0, toStack(ii));
                }
            }
        }

        private void BC_BefungeStackChanged(int value, BefungeCode.StackChangeType st)
        {
            switch (st)
            {
                case BefungeCode.StackChangeType.Add:
                    BfStack.Push(value);
                    break;
                case BefungeCode.StackChangeType.Remove:
                    if (BfStack.Count > 0)
                    {
                        BfStack.Pop();
                    }
                    break;
                case BefungeCode.StackChangeType.Clear:
                    BfStack.Clear();
                    break;
            }
        }

        private string toStack(int value)
        {
            if(value>32 && value<127)
            {
                return string.Format("{0} ({1})", value, ((char)value).ToString());
            }
            else
            {
                return string.Format("{0} (0x{1})",value,value.ToString("X"));
            }
        }

        private void BC_BefungeOutput(object value, BefungeCode.OutputType vt)
        {
            switch (vt)
            {
                case BefungeCode.OutputType.Char:
                    if ((int)value == 10)
                    {
                        console += "\r\n";
                    }
                    else
                    {
                        console += (char)(int)value;
                    }
                    break;
                case BefungeCode.OutputType.Number:
                    console += ((int)value).ToString() + " ";
                    break;
            }
        }

        private void frmExec_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BC != null)
            {
                BC.BefungeOutput -= Bout;
                BC.BefungeStackChanged -= Bstack;
            }
        }
    }
}
