/* UserInterface.cs
 * Author: Calvin Beechner
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.SudokuSolver
{
    public class UserInterface
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
        private TextBox[,] _textBox;

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
            int row = (int)tb.Name[0]-'0';
            int column = (int)tb.Name[1]-'0';
            if (tb.Text == "")
            {
                _puzzle[row, column] = 0;
            }
            else if (tb.Text.Length == 1 && (tb.Text[0] >= '1' && tb.Text[0] <= '9')) //may need to alter upper range
            {
                _puzzle[row, column] = Convert.ToInt32(tb.Text); //may lower range later
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
        /// 
        /// </summary>
        /// <param name="numberOfRowsAndColumns">An int giving the number of rows/columns in a single block of the puzzle.</param>
        private void AddPanelsToForm(int numberOfRowsAndColumns) //7.3.3
        /*
        This method should take as its only parameter an int giving the number of rows/columns in a single block of the puzzle. 
        It should return nothing. It is responsible for setting up FlowLayoutPanels to hold the blocks within the FlowLayoutPanel 
        defined through the Design window (see Section 3.2 Manual Design).

        You can access the contents of a FlowLayoutPanel via its Controls property. The object returned by this property has a Clear method 
        that takes no parameters and removes all controls from the panel. It also has an Add method that takes a Control as its only 
        parameter and adds that control to the panel. The controls are laid out in an order determined by the panel's FlowDirection property. 
        The FlowDirection property of the panel added via the Design window should have already been set to TopDown (via the Design window). 
        Each of the panels constructed here will need a FlowDirection of LeftToRight, which is the default.

        First, remove all controls from the FlowLayoutPanel defined through the Design window. Then use a loop to set up a FlowLayoutPanel for 
        each row of blocks. Set the following properties for each of these panels:

        AutoSize: true.
        AutoSizeMode: AutoSizeMode.GrowAndShrink.
        WrapContents: false.
        Margin: new Padding(0).
        Then add the panel to the panel defined through the Design window.

        Use an inner loop to add a FlowLayoutPanel for each block within a row. The only property of these panels that you will need to set here is 
        the Margin property. Construct a new Padding for this property as above, but in this case, use for its parameter the constant giving the size 
        of the margin to leave around a block. (You will set the sizes of these panels in another method).
        */
        {
            uxFlowLayoutPanel1.Controls.Clear();


        }
    }
}
