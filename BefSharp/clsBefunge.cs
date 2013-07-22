using System;
using System.Collections.Generic;
using System.Text;

namespace BefSharp
{
    public delegate void BefungeOutputHandler(object value,BefungeCode.OutputType vt);
    public delegate void BefungeStackChangedHandler(int value,BefungeCode.StackChangeType st);
    public class BefungeCode
    {
        /// <summary>
        /// This gets fired every time, "." or "," or "p" outputs something.
        /// </summary>
        public event BefungeOutputHandler BefungeOutput;
        /// <summary>
        /// Gets called each time the stack changes
        /// </summary>
        public event BefungeStackChangedHandler BefungeStackChanged;

        public const int W = 80;
        public const int H = 25;

        /// <summary>
        /// Directions, the code can go
        /// </summary>
        public enum Directions : int
        {
            /// <summary>
            /// Up
            /// </summary>
            North=1,
            /// <summary>
            /// Right
            /// </summary>
            East=2,
            /// <summary>
            /// Down
            /// </summary>
            South=3,
            /// <summary>
            /// Left
            /// </summary>
            West=4,
            /// <summary>
            /// Terminated
            /// </summary>
            Stop=0
        }
        /// <summary>
        /// Output Types of the BefungeOutput event
        /// </summary>
        public enum OutputType : int
        {
            /// <summary>
            /// Integer
            /// </summary>
            Number,
            /// <summary>
            /// Char
            /// </summary>
            Char,
            /// <summary>
            /// Code (after change with P)
            /// </summary>
            Code
        }
        /// <summary>
        /// Return values from the clock function
        /// </summary>
        public enum Actions : int
        {
            /// <summary>
            /// Next call requires Integer input
            /// </summary>
            InputInteger,
            /// <summary>
            /// Next call requires char input
            /// </summary>
            InputChar,
            /// <summary>
            /// No action on next call
            /// </summary>
            None,
            /// <summary>
            /// Application was terminated
            /// </summary>
            Terminated
        }
        /// <summary>
        /// Types of stack changement
        /// </summary>
        public enum StackChangeType : int
        {
            Add=1,
            Remove=2,
            Clear=0
        }
        /// <summary>
        /// X and Y coordinates container.
        /// </summary>
        public struct Position
        {
            public int X;
            public int Y;
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private char[] BfGrid;
        private Random R;

        /// <summary>
        /// Actual stack.
        /// Can be changed mid-execution
        /// </summary>
        public Stack<int> BfStack
        { get; set; }
        /// <summary>
        /// Actual Direction.
        /// Can be changed mid-execution
        /// </summary>
        public Directions Dir
        { get; set; }

        /// <summary>
        /// True, if in string mode.
        /// Can be changed mid-execution
        /// </summary>
        public bool StringMode
        { get; set; }

        /// <summary>
        /// Actual location in code (0,0) is top left.
        /// Can be changed mid-execution
        /// </summary>
        public Position CurrentInstruction;

        /// <summary>
        /// Actual code field.
        /// Can be changed mid-execution
        /// </summary>
        public string Code
        {
            get
            {
                StringBuilder SB = new StringBuilder(W * H+(H*2));
                for (int i = 0; i < H; i++)
                {
                    for (int j = 0; j < W; j++)
                    {
                        if (BfGrid[i * W + j] > 31 && BfGrid[i * W + j] < 128)
                        {
                            SB.Append(BfGrid[i * W + j]);
                        }
                        else
                        {
                            SB.Append('.');
                        }
                    }
                    SB.Append("\r\n");
                }
                return SB.ToString();
            }
            set
            {
                string[] Lines = FormatCode(value).Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < H; i++)
                {
                    for (int j = 0; j < W; j++)
                    {
                        BfGrid[i * W + j] = Lines[i][j];
                    }
                }
            }
        }

        /// <summary>
        /// Initializes new Befunge Code
        /// </summary>
        /// <param name="BfCode">initial Code to start with</param>
        public BefungeCode(string BfCode)
        {
            BfGrid = new char[W * H];
            BfStack = new Stack<int>();
            Dir = Directions.East;
            Code = BfCode;
            CurrentInstruction = new Position(0, 0);
            StringMode = false;
            R = new Random();
            BefungeOutput += new BefungeOutputHandler(BefungeCode_BefungeOutput);
            BefungeStackChanged+=new BefungeStackChangedHandler(BefungeCode_BefungeStackChanged);
        }

        void  BefungeCode_BefungeStackChanged(int value, BefungeCode.StackChangeType st)
        {
            //NULL HANDLER
            //A Null Handler makes executing an Event easier because there is no check
            //against 'null' needed. Also they serve great debugging purpose
#if DEBUG
            /*
            System.Diagnostics.Debug.WriteLine("#DEBUG Stack change: "+st.ToString());
            //*/
#endif
        }

        private void BefungeCode_BefungeOutput(object value, BefungeCode.OutputType vt)
        {
            //NULL HANDLER
            //A Null Handler makes executing an Event easier because there is no check
            //against 'null' needed. Also they serve great debugging purpose
#if DEBUG
            /*
            switch(vt)
            {
                case OutputType.Char:
                    System.Diagnostics.Debug.Write((char)(int)value);
                    break;
                case OutputType.Number:
                    System.Diagnostics.Debug.Write((int)value);
                    break;
            }
            //*/
#endif
        }

        public Actions Clock()
        {
            return Clock(int.MinValue);
        }

        /// <summary>
        /// Simulates a clock cycle
        /// </summary>
        /// <param name="input">Supply an enteres char or integer to the cycle to use</param>
        public Actions Clock(int input)
        {
            int c1, c2;
            char current=BfGrid[CurrentInstruction.Y * W + CurrentInstruction.X];
            if (Dir != Directions.Stop)
            {
                if (StringMode)
                {
                    if (current != '"')
                    {
                        Push(current);
                    }
                    else
                    {
                        StringMode = false;
                    }
                }
                else
                {
                    switch (current)
                    {
                        case '0': //Push value to stack
                        case '1': //Push value to stack
                        case '2': //Push value to stack
                        case '3': //Push value to stack
                        case '4': //Push value to stack
                        case '5': //Push value to stack
                        case '6': //Push value to stack
                        case '7': //Push value to stack
                        case '8': //Push value to stack
                        case '9': //Push value to stack
                            BfStack.Push(BfGrid[CurrentInstruction.Y * W + CurrentInstruction.X]-48);
                            break;
                        case 'a': //Push Hex Digit
                        case 'A': //Push Hex Digit
                            BfStack.Push(10);
                            break;
                        case 'b': //Push Hex Digit
                        case 'B': //Push Hex Digit
                            BfStack.Push(11);
                            break;
                        case 'c': //Push Hex Digit
                        case 'C': //Push Hex Digit
                            BfStack.Push(12);
                            break;
                        case 'd': //Push Hex Digit
                        case 'D': //Push Hex Digit
                            BfStack.Push(13);
                            break;
                        case 'e': //Push Hex Digit
                        case 'E': //Push Hex Digit
                            BfStack.Push(14);
                            break;
                        case 'f': //Push Hex Digit
                        case 'F': //Push Hex Digit
                            BfStack.Push(15);
                            break;
                        case '@': //End Program
                            Dir = Directions.Stop;
                            break;
                        case '$': //Discard top of stack
                            Pop();
                            break;
                        case ':': //Duplicate top of stack
                            Push(Push(Pop()));
                            break;
                        case '\\': //Switch top values
                            c1 = Pop();
                            c2 = Pop();
                            Push(c1);
                            Push(c2);
                            break;
                        case '+': //Addition
                            c1 = Pop();
                            c1 = Pop() + c1;
                            Push(c1);
                            break;
                        case '-': //Subtraction
                            c1 = Pop();
                            c1 = Pop() - c1;
                            Push(c1);
                            break;
                        case '*': //Multiplication
                            c1 = Pop();
                            c1 = Pop() * c1;
                            Push(c1);
                            break;
                        case '%': //Modulo
                            c1 = Pop();
                            if (c1 == 0)
                            {
                                Pop();
                                Push(0);
                            }
                            else
                            {
                                c1 = Pop() % c1;
                                Push(c1);
                            }
                            break;
                        case '/': //Division
                            c1 = Pop();
                            if (c1 == 0)
                            {
                                Pop();
                                Push(0);

                            }
                            else
                            {
                                c1 = Pop() / c1;
                                Push(c1);
                            }
                            break;
                        case '!': //NOT
                            c1 = Pop();
                            Push(c1 == 0 ? 1 : 0);
                            break;
                        case '`': //>
                            c1 = Pop();
                            c2 = Pop();
                            Push(c2 > c1 ? 1 : 0);
                            break;
                        case '"': //String Mode
                            StringMode = true;
                            break;
                        case '.': //Output int
                            BefungeOutput(Pop(), OutputType.Number);
                            break;
                        case ',': //Output char
                            BefungeOutput(Pop(), OutputType.Char);
                            break;
                        case '~': //Input char
                            if (input > int.MinValue)
                            {
                                Push(input);
                            }
                            else
                            {
                                return Actions.InputChar;
                            }
                            break;
                        case '&': //Input integer
                            if (input > int.MinValue)
                            {
                                Push(input);
                            }
                            else
                            {
                                return Actions.InputInteger;
                            }
                            break;
                        case '#': //jump over next
                            NextDir();
                            break;
                        case '>': //Direction change
                            Dir = Directions.East;
                            break;
                        case '<': //Direction change
                            Dir = Directions.West;
                            break;
                        case '^': //Direction change
                            Dir = Directions.North;
                            break;
                        case 'v': //Direction change
                            Dir = Directions.South;
                            break;
                        case '_': //Horizontal if
                            Dir = Pop() == 0 ? Directions.East : Directions.West;
                            break;
                        case '|': //vertical if
                            Dir = Pop() == 0 ? Directions.South: Directions.North;
                            break;
                        case '?': //Random Direction
                            Dir = (Directions)(R.Next(4) + 1);
                            break;
                        case 'g': //Get char
                            c1 = Pop();
                            c2 = Pop();
                            try
                            {
                                Push(BfGrid[ToCoord(c1, H) * W + ToCoord(c2, W)]);
                            }
                            catch
                            {
                                Push(0);
                            }
                            break;
                        case 'p': //put char
                            c1 = Pop();
                            c2 = Pop();
                            BfGrid[ToCoord(c1,H) * W + ToCoord(c2,W)] = (char)Pop();
                            BefungeOutput(Code, OutputType.Code);
                            break;

                    }
                }
            }
            if (Dir != Directions.Stop)
            {
                NextDir();
            }
            else
            {
                return Actions.Terminated;
            }
            return Actions.None;
        }

        private int ToCoord(int num, int uBound)
        {
            while (num < 0)
            {
                num += uBound;
            }
            while (num >= uBound)
            {
                num -= uBound;
            }
            return num;
        }

        private int Push(int c)
        {
            BfStack.Push(c);
            BefungeStackChanged(c, StackChangeType.Add);
            return c;
        }

        private int Pop()
        {
            if (BfStack.Count > 0)
            {
                int c = BfStack.Pop();
                BefungeStackChanged(c, StackChangeType.Remove);
                return c;
            }
            BefungeStackChanged(0, StackChangeType.Clear);
            return 0;
        }

        private void NextDir()
        {
            switch (Dir)
            {
                case Directions.North:
                    if (CurrentInstruction.Y > 0)
                    {
                        CurrentInstruction.Y--;
                    }
                    else
                    {
                        CurrentInstruction.Y = H-1;
                    }
                    break;
                case Directions.East:
                    if (CurrentInstruction.X < W-1)
                    {
                        CurrentInstruction.X++;
                    }
                    else
                    {
                        CurrentInstruction.X = 0;
                    }
                    break;
                case Directions.South:
                    if (CurrentInstruction.Y < H-1)
                    {
                        CurrentInstruction.Y++;
                    }
                    else
                    {
                        CurrentInstruction.Y = 0;
                    }
                    break;
                case Directions.West:
                    if (CurrentInstruction.X > 0)
                    {
                        CurrentInstruction.X--;
                    }
                    else
                    {
                        CurrentInstruction.X = W - 1;
                    }
                    break;
                case Directions.Stop:
                    break;
                default:
                    throw new Exception(string.Format("Unknown direction: {0}",Dir.ToString()));
            }
        }

        /// <summary>
        /// Formats code in 80x25 befunge style
        /// </summary>
        /// <param name="Code">Code</param>
        /// <returns>Formatted code</returns>
        public static string FormatCode(string Code)
        {
            StringBuilder SB = new StringBuilder(W * H + 2);
            //Uniform Linebreaks and split by lines
            string[] Lines = Code.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            if (Lines.Length != H)
            {
                Array.Resize<string>(ref Lines, H);
                for (int i = 0; i < H; i++)
                {
                    if (string.IsNullOrEmpty(Lines[i]))
                    {
                        Lines[i] = string.Empty;
                    }
                }
            }
            for (int i = 0; i < H; i++)
            {
                while (Lines[i].Length < W)
                {
                    Lines[i] += ' ';
                }
                if (Lines[i].Length > W)
                {
                    Lines[i] = Lines[i].Substring(0, W);
                }
            }
            foreach (string Line in Lines)
            {
                SB.AppendLine(Line);
            }
            return SB.ToString();
        }
    }
}
