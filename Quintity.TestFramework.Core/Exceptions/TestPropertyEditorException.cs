using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestPropertyEditorException : ApplicationException
    {
        public int RowIndex
        { get; set; }

        public int ColumnIndex
        { get; set; }

        public TestPropertyEditorException()
        {
        }

        public TestPropertyEditorException(int rowIndex, int columnIndex, string message)
            : base(message)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
    }
}
