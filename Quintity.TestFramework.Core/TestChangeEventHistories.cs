using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Quintity.TestFramework.Core
{
    public class TestChangeEventHistories
    {
        #region Event declarations

        public delegate void TestHistoryUndoEvent(TestChangeEvent e);
        public event TestHistoryUndoEvent OnTestHistoryUndoEvent;

        public delegate void TestHistoryRedoEvent(TestChangeEvent e);
        public event TestHistoryRedoEvent OnTestHistoryRedoEvent;

        #endregion

        #region private members

        private Stack<TestChangeEvent> m_undoStack;
        private Stack<TestChangeEvent> m_redoStack;

        #endregion

        #region constructors

        public TestChangeEventHistories()
        {
            m_undoStack = new Stack<TestChangeEvent>();
            m_redoStack = new Stack<TestChangeEvent>();
        }

        #endregion

        #region public methods

        public bool CanUndo()
        {
            return m_undoStack.Count > 0 ? true : false;
        }

        public bool CanRedo()
        {
            return m_redoStack.Count > 0 ? true : false;
        }

        public void RecordEvent(TestChangeEvent changeEvent)
        {
            Debug.WriteLine("Event recorded:  " + changeEvent.ToString());

            m_undoStack.Push(changeEvent);
        }

        public void Undo()
        {
            TestChangeEvent changeEvent = null;

            if (m_undoStack.Count > 0)
            {
                changeEvent = m_undoStack.Pop();

                Debug.WriteLine("Undo event popped:  " + changeEvent.ToString());

                createTestHistoryUndoEvent(changeEvent);

                m_redoStack.Push(changeEvent);
            }
        }

        public void Redo()
        {
             if (m_redoStack.Count > 0)
             {
                 TestChangeEvent changeEvent = m_redoStack.Pop();

                 createTestHistoryRedoEvent(changeEvent);

                 m_undoStack.Push(changeEvent);
             }
        }

        // Clears the undo/redo stack.
        public void Reset()
        {
            m_redoStack.Clear();
            m_undoStack.Clear();
        }

        #endregion

        /// <summary>
        /// Creates a createTestHistoryUndoEvent for undo entity.
        /// </summary>
        /// <param name="testObject">Entity from undo stack.</param>
        private void createTestHistoryUndoEvent(TestChangeEvent entity)
        {
            // If client event handler defined...
            if (OnTestHistoryUndoEvent != null)
            {
                OnTestHistoryUndoEvent(entity);
            }
        }

        /// <summary>
        /// Creates a createTestHistoryRedoEvent for undo entity.
        /// </summary>
        /// <param name="testObject">Entity from undo stack.</param>
        private void createTestHistoryRedoEvent(TestChangeEvent entity)
        {
            // If client event handler defined...
            if (OnTestHistoryRedoEvent != null)
            {
                OnTestHistoryRedoEvent(entity);
            }
        }
    }
}
