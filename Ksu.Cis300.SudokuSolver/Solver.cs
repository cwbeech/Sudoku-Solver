/* Solver.cs
 * Author: Calvin Beechner
 */
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ksu.Cis300.SudokuSolver
{
    public static class Solver
    {
        /// <summary>
        /// A int[,] used to store the puzzle. A 0 in array location will represent an unfilled cell.
        /// </summary>
        private static int[,] _puzzle;

        /// <summary>
        /// An int representing the number of rows and columns in the puzzle. This will always be either 4 or 9.
        /// </summary>
        private static int _numberOfRowsAndColumnsInPuzzle;

        /// <summary>
        /// An int representing the number of rows and columns in a block. This will always be either 2 or 3.
        /// </summary>
        private static int _numberOfRowsAndColumnsInBlock;

        /// <summary>
        /// A bool[,] used to track the values used in each row.
        /// </summary>
        private static bool[,] _valuesUsedInRow = new bool[_numberOfRowsAndColumnsInPuzzle, 
                                                           _numberOfRowsAndColumnsInPuzzle+1];

        /// <summary>
        /// A bool[,,] used to track the values used in each block.
        /// </summary>
        private static bool[,,] _valuesUsedInBlock = new bool[_numberOfRowsAndColumnsInBlock, 
                                                              _numberOfRowsAndColumnsInBlock, 
                                                              _numberOfRowsAndColumnsInBlock * _numberOfRowsAndColumnsInBlock + 1];

        /// <summary>
        /// A DoulbyLinkedListCell[] used to store the empty puzzle locations in each row.
        /// </summary>
        private static DoublyLinkedListCell[] _emptyPuzzleLocations;

        /// <summary>
        /// A DoublyLinkedListCell[] used to store the unused values for each row.
        /// </summary>
        private static DoublyLinkedListCell[] _unusedValues;

        /// <summary>
        /// An int used to store the current row being filled.
        /// </summary>
        private static int _currentRow;

        /// <summary>
        /// A Stack<DoublyLinkedListCell> used to contain the cells removed from the lists of unused values.
        /// </summary>
        private static Stack<DoublyLinkedListCell> _cellsRemovedFromListsUnused = new Stack<DoublyLinkedListCell>();

        /// <summary>
        /// A DoublyLinkedListCell used to refer to the cell giving the empty puzzle location to be filled next.
        /// </summary>
        private static DoublyLinkedListCell _nextEmptyLocation = new DoublyLinkedListCell();

        /// <summary>
        /// A DoublyLinkedListCell used to refer to the cell containing the next value to be tried.
        /// </summary>
        private static DoublyLinkedListCell _nextTriedValue = new DoublyLinkedListCell();

        /// <summary>
        /// Removes a DoulbyLinkedListCell from a DoublyLinkedList.
        /// </summary>
        /// <param name="dllc">The DoublyLinkedListCell being removed.</param>
        public static void RemoveCellFromList(DoublyLinkedListCell dllc)
        {
            dllc.Previous.Next = dllc.Next; //removes previous's reference to deleted cell
            dllc.Next.Previous = dllc.Previous.Previous; //removes next's reference to deleted cell
        }

        /// <summary>
        /// Restores a removed DoublyLinkedListCell.
        /// </summary>
        /// <param name="dllc">The DoublyLinkedListCell being restored.</param>
        private static void RestoreRemovedCell(DoublyLinkedListCell dllc)
        {
            dllc.Previous.Next = dllc;
            dllc.Next.Previous = dllc;
        }

        /// <summary>
        /// Inserts a DoublyLinkedListCell into a DoublyLinkedList.
        /// </summary>
        /// <param name="first">The DoublyLinkedListCell being inserted.</param>
        /// <param name="second">The DoublyLinkedListCell following the inserted Cell.</param>
        private static void InsertCellIntoList(DoublyLinkedListCell first, DoublyLinkedListCell second)
        {
            second.Previous.Next = first;
            first.Previous = second.Previous;
            second.Previous = first;
            first.Next = second;
        }

        /// <summary>
        /// Creates and returns a DoublyLinkedListCell containing all valid values that can be used in a puzzle.
        /// </summary>
        /// <returns>DoublyLinkedListCell containing all valid values.</returns>
        private static DoublyLinkedListCell GetListOfAllPuzzleValues()
        {
            /*
            DoublyLinkedListCell result = new DoublyLinkedListCell();
            result.Data = 9;
            for (int i = 8; i >= _numberOfRowsAndColumnsInPuzzle; i--)
            {
                DoublyLinkedListCell temp = result;
                DoublyLinkedListCell cell = new DoublyLinkedListCell();
                cell.Data = i;
                cell.Next = temp;
                temp.Previous = cell;
                result = cell;
            }
            return result;
            //this is wrong.
            */
            DoublyLinkedListCell header = new DoublyLinkedListCell();
            for (int i = 1; i < _numberOfRowsAndColumnsInPuzzle; i++)
            {
                DoublyLinkedListCell temp = new DoublyLinkedListCell();
                temp.Data = i;
                InsertCellIntoList(temp, header);
            }
            return header;
        }

        /// <summary>
        /// Removes a given value from a list of puzzle values.
        /// </summary>
        /// <param name="value">Value to be removed.</param>
        /// <param name="header">Header of the list from which the value is to be removed.</param>
        private static bool RemoveValueFromList(int value, DoublyLinkedListCell header)
        {
            bool result = false;
            DoublyLinkedListCell iterator = header.Next;
            bool found = false;
            while (!found && iterator != header) //while the value is not found AND not header.
            {
                if (iterator.Data != value)
                {
                    iterator = iterator.Next;
                }
                else
                {
                    found = true;
                }
            }
            if (found)
            {
                RemoveCellFromList(iterator);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Records the presense or absence of a value in the two boolean arrays.
        /// </summary>
        /// <param name="row">Int representing the row in which the value occurs.</param>
        /// <param name="column">Int representing the column in which the value occurs.</param>
        /// <param name="value">Int representing the value.</param>
        /// <param name="isPresent">Bool indicating whether the value is present.</param>
        private static void RecordStatusOfValueInBoolArrays(int row, int column, int value, bool isPresent)
        {
            _valuesUsedInRow[row, column] = isPresent;
            _valuesUsedInBlock[row/_numberOfRowsAndColumnsInBlock, column/_numberOfRowsAndColumnsInBlock, value] = isPresent; //maybe check on this one later, could be wrong.
        }

        /// <summary>
        /// Determines if a value can be legally placed at a given location.
        /// </summary>
        /// <param name="row">Row in which the value is to be placed.</param>
        /// <param name="column">Column in which the value is to be placed.</param>
        /// <param name="value">The value in questionl.</param>
        /// <returns></returns>
        private static bool CanBeLegallyPlacedAtLocation(int row, int column, int value)
        {
            bool result = true;
            if (_valuesUsedInRow[row, column])
            {
                result = false;
            }
            else if (_valuesUsedInBlock[row/_numberOfRowsAndColumnsInBlock, column/_numberOfRowsAndColumnsInBlock, value])
            {
                result = false;
            }
            return result;
        }


        private static bool InitializeOneRow(int row) //stopped at 7.2.9
        {
            DoublyLinkedListCell EmptyHeader = new DoublyLinkedListCell();
            for (int i = 0; i < _emptyPuzzleLocations.Length; i++)
            {
                DoublyLinkedListCell temp = _emptyPuzzleLocations[i];
                InsertCellIntoList(temp, EmptyHeader);
            }
            DoublyLinkedListCell unused = GetListOfAllPuzzleValues();


            return false;
        }
    }
}
