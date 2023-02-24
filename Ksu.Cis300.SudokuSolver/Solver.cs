/* Solver.cs
 * Author: Calvin Beechner
 */
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
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
        private static bool[,] _valuesUsedInRow;

        /// <summary>
        /// A bool[,,] used to track the values used in each block.
        /// </summary>
        private static bool[,,] _valuesUsedInBlock;

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
        private static Stack<DoublyLinkedListCell> _cellsRemovedFromListsUnused;

        /// <summary>
        /// A DoublyLinkedListCell used to refer to the cell giving the empty puzzle location to be filled next.
        /// </summary>
        private static DoublyLinkedListCell _currentLocation;

        /// <summary>
        /// A DoublyLinkedListCell used to refer to the cell containing the next value to be tried.
        /// </summary>
        private static DoublyLinkedListCell _nextValue;

        /// <summary>
        /// Removes a DoulbyLinkedListCell from a DoublyLinkedList.
        /// </summary>
        /// <param name="dllc">The DoublyLinkedListCell being removed.</param>
        public static void RemoveCellFromList(DoublyLinkedListCell dllc)
        {
            dllc.Previous.Next = dllc.Next; //removes previous's reference to deleted cell
            dllc.Next.Previous = dllc.Previous; //removes next's reference to deleted cell
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
            first.Previous = second.Previous;
            first.Next = second;
            RestoreRemovedCell(first);
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
            for (int i = 1; i <= _numberOfRowsAndColumnsInPuzzle; i++)
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
            _valuesUsedInRow[row, value] = isPresent;
            _valuesUsedInBlock[row/_numberOfRowsAndColumnsInBlock, column/_numberOfRowsAndColumnsInBlock, value] = isPresent; //maybe check on this one later, could be wrong.
        }

        /// <summary>
        /// Determines if a value can be legally placed at a given location.
        /// </summary>
        /// <param name="row">Row in which the value is to be placed.</param>
        /// <param name="column">Column in which the value is to be placed.</param>
        /// <param name="value">The value in question.</param>
        /// <returns></returns>
        private static bool CanBeLegallyPlacedAtLocation(int row, int column, int value)
        {
            bool result = true;
            
            
            if (_valuesUsedInRow[row, value])
            {
                result = false;
            }
            for (int i = 0; i < _numberOfRowsAndColumnsInPuzzle; i++)
            {
                if (_puzzle[i, column] == value)
                {
                    if (i != row)
                    {
                        result = false;
                    }
                }
            }
            if (_valuesUsedInBlock[row/_numberOfRowsAndColumnsInBlock, column/_numberOfRowsAndColumnsInBlock, value])
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Initialized one row of the puzzle
        /// </summary>
        /// <param name="row"></param>
        /// <returns>AN int representing the index of the row being initialized</returns>
        private static bool InitializeOneRow(int row)
        {
            DoublyLinkedListCell EmptyHeader = new DoublyLinkedListCell();
            _emptyPuzzleLocations[row] = EmptyHeader;
            _unusedValues[row] = GetListOfAllPuzzleValues();

            for (int i = 0; i < _puzzle.GetLength(1); i++)
            {
                if (_puzzle[row, i] == 0)
                {
                    DoublyLinkedListCell temp = new DoublyLinkedListCell();
                    temp.Data = i;//previously temp.Data = _puzzle[row][i];
                    InsertCellIntoList(temp, EmptyHeader);
                }
                else
                {
                    if (!(CanBeLegallyPlacedAtLocation(row, i, _puzzle[row, i]) || RemoveValueFromList(_puzzle[row, i], EmptyHeader)))
                    {
                        return false;
                    }
                    RecordStatusOfValueInBoolArrays(row, i, _puzzle[row, i], true);
                    /*
                    if (!CanBeLegallyPlacedAtLocation(row, i, _puzzle[row, i]))
                    {
                        if (!RemoveValueFromList(_puzzle[row, i], EmptyHeader))
                        {
                            return false;
                        }
                    }
                    RecordStatusOfValueInBoolArrays(row, i, _puzzle[row, i], true);
                    */
                }
            }
            return true;
        }

        /// <summary>
        /// Initializes all fields except for last two used in backtracking.
        /// </summary>
        /// <param name="puzzle">The puzzle to be solved.</param>
        /// <returns>Bool representing whether the initialization passed.</returns>
        private static bool InitializeFields(int[,] puzzle)
        {
            _puzzle = puzzle;
            _numberOfRowsAndColumnsInPuzzle = puzzle.GetLength(0);
            _numberOfRowsAndColumnsInBlock = (int)Math.Sqrt((double)puzzle.GetLength(0));
            _valuesUsedInRow = new bool[_numberOfRowsAndColumnsInPuzzle, _numberOfRowsAndColumnsInPuzzle + 1];
            _valuesUsedInBlock = new bool[_numberOfRowsAndColumnsInBlock, _numberOfRowsAndColumnsInBlock, _numberOfRowsAndColumnsInPuzzle + 1];
            _emptyPuzzleLocations = new DoublyLinkedListCell[_numberOfRowsAndColumnsInPuzzle];
            _unusedValues = new DoublyLinkedListCell[_numberOfRowsAndColumnsInPuzzle];
            for (int i = 0; i < _numberOfRowsAndColumnsInPuzzle; i++)
            {
                if (!InitializeOneRow(i))
                {
                    return false;
                }
            }
            _cellsRemovedFromListsUnused = new Stack<DoublyLinkedListCell>();
            return true;
        }

        /// <summary>
        /// Advances current row.
        /// </summary>
        /// <returns>Bool representing whether or not it successfully advanced.</returns>
        private static bool AdvanceToNextRow()
        {
            _currentRow++;
            if (_currentRow >= _numberOfRowsAndColumnsInPuzzle) //advancing the current row field to the next row
            {
                return false;
            }
            _currentLocation = _emptyPuzzleLocations[_currentRow].Next; //setting the current location to the first empty location of the new row.
            _nextValue = _unusedValues[_currentRow].Next; //setting the next value to the first unused value on the mew row.
            return true;
        }

        /// <summary>
        /// Initiates Bakctrack algorithm. If false solution doesn't exist.
        /// </summary>
        /// <returns>A bool representing whether or not the backtracking worked.</returns>
        private static bool Backtrack()
        {
            while(_cellsRemovedFromListsUnused.Count > 0)
            {
                if (_currentLocation.Previous == _emptyPuzzleLocations[_currentRow])
                {
                    _currentRow--;
                    _currentLocation = _emptyPuzzleLocations[_currentRow];
                }
                else
                {
                    _currentLocation = _currentLocation.Previous;
                    DoublyLinkedListCell topCell = _cellsRemovedFromListsUnused.Pop();
                    _puzzle[_currentRow, _currentLocation.Data] = 0;
                    RecordStatusOfValueInBoolArrays(_currentRow, _currentLocation.Data, topCell.Data, false);
                    RestoreRemovedCell(topCell);
                    if (topCell != _unusedValues[_currentRow])
                    {
                        _nextValue = topCell.Next;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Uses the next value in the current location.
        /// </summary>
        private static void UseNextValue()
        {
            if (CanBeLegallyPlacedAtLocation(_currentRow, _currentLocation.Data, _nextValue.Data))
            {
                DoublyLinkedListCell temp = _nextValue;
                RemoveCellFromList(_nextValue);
                _cellsRemovedFromListsUnused.Push(temp);
                _puzzle[_currentRow, _currentLocation.Data] = temp.Data;
                RecordStatusOfValueInBoolArrays(_currentRow, _currentLocation.Data, temp.Data, true); //what to use for column.
                _nextValue = _unusedValues[_currentRow].Next;
                _currentLocation = _currentLocation.Next;
            }
            else
            {
                _nextValue = _nextValue.Next;
            }
        }

        /// <summary>
        /// Solves a puzzle.
        /// </summary>
        /// <param name="puzzle">The puzzle being solved.</param>
        /// <returns>A bool representing whether the puzzle was successfully solved.</returns>
        public static bool Solve(int[,] puzzle)
        {
            if (!InitializeFields(puzzle))
            {
                return false;
            }
            _currentRow = -1;
            AdvanceToNextRow();
            while (true)
            {
                if (_currentLocation == _emptyPuzzleLocations[_currentRow])
                {
                    if (!AdvanceToNextRow())
                    {
                        return true;
                    }
                }
                else if (_nextValue == _unusedValues[_currentRow])
                {
                    if (!Backtrack())
                    {
                        return false;
                    }
                }
                else
                {
                    UseNextValue();
                }
            }
        }
    }
}
