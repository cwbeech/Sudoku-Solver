/* DoublyLinkedListCell.cs
 * Author: Calvin Beechner
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksu.Cis300.SudokuSolver
{
    public class DoublyLinkedListCell
    {
        /// <summary>
        /// A property to get or set an int giving the data stored in the cell.
        /// </summary>
        public int Data { get; set; }

        /// <summary>
        /// A property to get or set the previous DoublyLinkedListCell in the list.
        /// </summary>
        public DoublyLinkedListCell Previous { get; set; }

        /// <summary>
        /// A property to get or set the next DoublyLinkedListCell in the list.
        /// </summary>
        public DoublyLinkedListCell Next { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DoublyLinkedListCell()
        {
            Previous = this;
            Next = this;
        }
    }
}
