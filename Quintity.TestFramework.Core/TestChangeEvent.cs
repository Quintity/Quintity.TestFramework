using System;
using System.Collections.Generic;
using System.Text;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Core
{
    public class TestScriptObjectLocation
    {
        public TestScriptObjectContainer Parent
        { get; set; }

        public int Index
        { get; set; }

        public TestScriptObjectLocation(TestScriptObjectContainer parent, int index)
        {
            Parent = parent;
            Index = index;
        }

        public override string ToString()
        {
            return string.Format("Parent node:  {0}, Index:  {1}",
                Parent != null ? Parent.Title : "N/A", Index);
        }
    }

    public class TestChangeEvent
    {
        private TestScriptObject m_testScriptObject;
        private ChangeType m_changeType;
        private string m_property;
        private object m_currentState;
        private object m_formerState;
        private object m_tag;

        public TestChangeEvent(TestScriptObject testScriptObject, ChangeType editAction, object formerValue, object tag = null)
            : this(testScriptObject, editAction, null, null, formerValue, tag)
        { }

        public TestChangeEvent(TestScriptObject testScriptObject, ChangeType editAction, object currentValue, object formerValue, object tag = null)
            : this(testScriptObject, editAction, null, currentValue, formerValue, tag)
        { }

        public TestChangeEvent(TestScriptObject testScriptObject, ChangeType editAction,
            string property, object currentValue, object formerValue, object tag = null)
        {
            this.m_testScriptObject = testScriptObject;
            this.m_changeType = editAction;
            this.m_property = property;
            this.m_currentState = currentValue;
            this.m_formerState = formerValue;
            this.m_tag = tag;
        }

        public TestScriptObject TestScriptObject
        {
            get { return this.m_testScriptObject; }
        }

        public ChangeType ChangeType
        {
            get { return this.m_changeType; }
        }

        public string Property
        {
            get { return this.m_property; }
        }

        public object CurrentValue
        {
            get { return this.m_currentState; }
        }

        public object FormerValue
        {
            get { return this.m_formerState; }
        }

        public object Tag
        {
            get { return m_tag; }
        }

        public override string ToString()
        {
            return string.Format("TestScriptObject:  {0}, Action:  {1}, Property:  {2}, Current: {3}, Former:  {4}, Tag:  {5}",
                m_testScriptObject.Title, m_changeType,
                m_property  != null ? m_property : "N/A",
                m_currentState != null ? m_currentState.ToString() : "N/A",
                m_formerState != null ? m_formerState.ToString() : "N/A",
                m_tag != null ? m_tag.ToString() : "N/A");
        }

    }
}
