using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.SudokuSolver
{
    public partial class UserInterface : Form
    {
        /// <summary>
        /// Stores the number of rows/columns in a single block of a 4x4 puzzle.
        /// </summary>
        private const int _numberOfRowColumnsInBlockFour = 2;

        /// <summary>
        /// Stores the number of rows/columns in a single block of a 9x9 puzzle.
        /// </summary>
        private const int _numberOfRowColumnsInBlockNine = 3;

        /// <summary>
        /// Stores the size of the margin to leave around a block with the puzzle.
        /// </summary>
        private const int _sizeOfMargin = 1;

        /// <summary>
        /// Stores the font size for the cells in the puzzle.
        /// </summary>
        private const int _fontSize = 18;

        /// <summary>
        /// A Font giving the font to use for values entered by the user.
        /// </summary>
        private readonly Font _fontValuesOfUser = new Font(FontFamily.GenericSerif, _fontSize, FontStyle.Bold);

        /// <summary>
        /// A Font giving the font to use for the values supplied by the program.
        /// </summary>
        private readonly Font _fontValueOfProgram = new Font(FontFamily.GenericSerif, _fontSize);

        /// <summary>
        /// A TextBox[,] to hold the TextBoxes forming the puzzle.
        /// </summary>
        private TextBox[,] _textBoxes;

        /// <summary>
        /// An int[,] to store the puzzle in the format needed by the Solver Class.
        /// </summary>
        private int[,] _puzzle;

        /// <summary>
        /// Handles a TextChanged event on a TextBox.
        /// </summary>
        /// <param name="sender">Refers to the TextBox which signaled the event.</param>
        /// <param name="e"></param>
        private void CellTextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int row = (int)tb.Name[0] - '0';
            int column = (int)tb.Name[1] - '0';
            char  rangee = _puzzle.GetLength(0).ToString()[0];
            if (tb.Text == "")
            {
                _puzzle[row, column] = 0;
            }
            else if (tb.Text.Length == 1 && (tb.Text[0] >= '1' && tb.Text[0] <= rangee)) //may need to alter upper range
            {
                _puzzle[row, column] = Convert.ToInt16(tb.Text);
            }
            else if (_puzzle[row, column] == 0)
            {
                tb.Text = "";
            }
            else
            {
                tb.Text = Convert.ToString(_puzzle[row, column]);
            }
        }

        /// <summary>
        /// Adds panels to the form.
        /// </summary>
        /// <param name="numberOfRowsAndColumns">An int giving the number of rows/columns in a single block of the puzzle.</param>
        private void AddPanelsToForm(int numberOfRowsAndColumns)
        {
            uxFlowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < numberOfRowsAndColumns; i++)
            {
                FlowLayoutPanel rowOfBlocks = new FlowLayoutPanel();
                rowOfBlocks.AutoSize = true;
                rowOfBlocks.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                rowOfBlocks.WrapContents = false;
                rowOfBlocks.Margin = new Padding(0);
                rowOfBlocks.Name = "trackRow"; //delete later
                uxFlowLayoutPanel1.Controls.Add(rowOfBlocks);
                for (int ii = 0; ii < numberOfRowsAndColumns; ii++)
                {
                    FlowLayoutPanel block = new FlowLayoutPanel();
                    block.Margin = new Padding(_sizeOfMargin);
                    block.Name = "trackBlock"; //delete later
                    rowOfBlocks.Controls.Add(block);
                }
            }

        }


        /// <summary>
        /// Adds the textBoxes.
        /// </summary>
        /// <param name="numberOfRowColumnsInBlock">An int giving the number of rows/columns in a single block.</param>
        private void AddTextBoxes(int numberOfRowColumnsInBlock) //fix
        {
            _textBoxes = new TextBox[numberOfRowColumnsInBlock * numberOfRowColumnsInBlock, numberOfRowColumnsInBlock * numberOfRowColumnsInBlock];
            _puzzle = new int[numberOfRowColumnsInBlock * numberOfRowColumnsInBlock, numberOfRowColumnsInBlock * numberOfRowColumnsInBlock];
            for (int i = 0; i < numberOfRowColumnsInBlock * numberOfRowColumnsInBlock; i++)
            {
                for (int ii = 0; ii < numberOfRowColumnsInBlock * numberOfRowColumnsInBlock; ii++)
                {
                    _textBoxes[i, ii] = new TextBox();
                    _textBoxes[i, ii].Font = _fontValuesOfUser;
                    _textBoxes[i, ii].Width = _textBoxes[i, ii].Height;
                    _textBoxes[i, ii].Margin = new Padding(0);
                    _textBoxes[i, ii].TextAlign = HorizontalAlignment.Center;
                    _textBoxes[i, ii].Name = i.ToString() + ii.ToString();
                    _textBoxes[i, ii].TextChanged += new EventHandler(CellTextChanged);
                    uxFlowLayoutPanel1.Controls[i / numberOfRowColumnsInBlock].Controls[ii/ numberOfRowColumnsInBlock].Controls.Add(_textBoxes[i, ii]);
                }
            }
        }

        /// <summary>
        /// Resizes the panels.
        /// </summary>
        /// <param name="numberOfRowColumnsInBlock">An int giving the number of rows/columns in a single block.</param>
        private void ResizePanels(int numberOfRowColumnsInBlock)
        {
            foreach (FlowLayoutPanel i in uxFlowLayoutPanel1.Controls)
            {
                foreach (FlowLayoutPanel ii in i.Controls)
                {

                    ii.Size = new Size(numberOfRowColumnsInBlock * _textBoxes[0, 0].Height, numberOfRowColumnsInBlock * _textBoxes[0, 0].Height);
                }
            }
        }

        /// <summary>
        /// Starts a new puzzle.
        /// </summary>
        /// <param name="numberOfRowColumnsInBlock">An int giving the number of rows/columns in a single block.</param>
        private void StartNewPuzzle(int numberOfRowColumnsInBlock)
        {
            AddPanelsToForm(numberOfRowColumnsInBlock);
            AddTextBoxes(numberOfRowColumnsInBlock);
            ResizePanels(numberOfRowColumnsInBlock);
            solveToolStripMenuItem.Enabled = true; //rename?
            uxFlowLayoutPanel1.Enabled = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();
            StartNewPuzzle(_numberOfRowColumnsInBlockNine); //change for future scaling
        }

        /// <summary>
        /// Event handler for "New 4x4" menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void new4x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewPuzzle(_numberOfRowColumnsInBlockFour);
        }

        /// <summary>
        /// Event handler for "New 9x9" menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void new9x9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewPuzzle(_numberOfRowColumnsInBlockNine);
        }


        /// <summary>
        /// Places a solution into the GUI.
        /// </summary>
        private void PlaceSolutionInGUI()
        {
            for (int i = 0; i < _puzzle.GetLength(0); i++)
            {
                for (int ii = 0; ii < _puzzle.GetLength(1); ii++)
                {
                    if (_textBoxes[i, ii].Text == "")
                    {
                        _textBoxes[i, ii].Text = _puzzle[i, ii].ToString();
                        _textBoxes[i, ii].Font = _fontValueOfProgram;
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for the "Solve" menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Solver.Solve(_puzzle);
            PlaceSolutionInGUI();
        }
    }
}

