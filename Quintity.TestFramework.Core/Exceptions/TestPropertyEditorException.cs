using System;

namespace Quintity.TestFramework.Core
{
    public class TestPropertyEditorException : TestException
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
