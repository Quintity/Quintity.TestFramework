/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    abstract public class TestScriptObjectContainer : TestScriptObject
    {
        #region Class data members

        [DataMember(Order = 31)]
        protected string _functionalArea;

        [CategoryAttribute("General"),
        DisplayName("Functional Area"),
        DescriptionAttribute("Product functional reference."),
        PropertyOrder(31)]
        [Editor(typeof(TestTextBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        virtual public string FunctionalArea
        {
            get { return _functionalArea; }
            set
            {
                if (_functionalArea != value)
                {
                    object oldValue = _functionalArea;
                    _functionalArea = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 32)]
        protected string _reference;

        [CategoryAttribute("General"),
        DescriptionAttribute("General reference identifier(s)."),
        PropertyOrder(32)]
        [Editor(typeof(TestTextBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        virtual public string Reference
        {
            get { return _reference; }
            set
            {
                if (_reference != value)
                {
                    object oldValue = _reference;
                    _reference = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 33)]
        protected string _author;

        [IgnoreDataMember]
        [CategoryAttribute("Metadata"),
        DescriptionAttribute("Test artifact author."),
        PropertyOrder(33)]
        virtual public string Author
        { get { return _author; } }

        [DataMember(Order = 34)]
        protected DateTime _created;

        [IgnoreDataMember]
        [CategoryAttribute("Metadata"),
        DescriptionAttribute("Date test artifact was created."),
        PropertyOrder(34)]
        virtual public DateTime Created
        { get { return _created; } }

        [DataMember(Order = 36)]
        protected TestPropertyCollection _testProperties;

        // Removed from UI until a) needed and b) updated to TestProperties format.
        [CategoryAttribute("Runtime Settings"),
        DisplayName("Test Properties"),
        DescriptionAttribute("Additional runtime properties"),
        PropertyOrder(30)]
        [Browsable(false)]
        //[Editor(typeof(TestPropertiesEditor), typeof(System.Drawing.Design.UITypeEditor))]
        virtual public TestPropertyCollection TestProperties
        {
            get { return _testProperties; }
            set
            {
                if (_testProperties != value)
                {
                    object oldValue = _testProperties;
                    _testProperties = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 40)]
        protected TestScriptObjectCollection _testScriptObjects;

        /// <summary>
        /// Returns the container's TestScriptObjectCollection.  Internal
        /// property for convenience.
        /// </summary>
        [IgnoreDataMember]
        [Browsable(false)]
        public TestScriptObjectCollection TestScriptObjects
        { get { return _testScriptObjects; } }

        #endregion

        #region Class constructors

        internal TestScriptObjectContainer()
            : this(null)
        { }

        public TestScriptObjectContainer(TestScriptObjectContainer originalTestScriptObjectContainer, TestScriptObjectContainer parent)
            : base(originalTestScriptObjectContainer, parent)
        {
            _functionalArea = TestUtils.SafeCopyString(originalTestScriptObjectContainer._functionalArea);
            _reference = TestUtils.SafeCopyString(originalTestScriptObjectContainer._reference);
            _author = Environment.UserName;
            _created = originalTestScriptObjectContainer._created;

            _testProperties = new TestPropertyCollection();

            foreach (TestProperty testProperty in originalTestScriptObjectContainer._testProperties)
            {
                _testProperties.Add(new TestProperty(testProperty));
            }

            _testScriptObjects = new TestScriptObjectCollection();

            foreach (TestScriptObject testScriptObject in originalTestScriptObjectContainer._testScriptObjects)
            {
                TestScriptObject newObject = null;

                if (testScriptObject is TestSuite)
                {
                    newObject = new TestSuite((TestSuite)testScriptObject, null, (TestSuite)this);
                }
                else if (testScriptObject is TestCase)
                {
                    newObject = new TestCase((TestCase)testScriptObject, (TestSuite)this);
                }
                else if (testScriptObject is TestStep)
                {
                    newObject = new TestStep((TestStep)testScriptObject, (TestCase)this);
                }

                _testScriptObjects.Add(newObject);
            }
        }

        public TestScriptObjectContainer(string title)
            : this(title, null)
        { }

        public TestScriptObjectContainer(string title, TestSuite parent)
            : base(title, parent)
        {
            _created = DateTime.Now;
            _author = Environment.UserName;
            _testScriptObjects = new TestScriptObjectCollection();
            _status = Core.Status.Active;
            _testProperties = new TestPropertyCollection();
        }

        #endregion

        #region Class public methods

        private TestScriptObject m_foundTestScriptObject;

        public TestScriptObject Find(Guid systemId)
        {
            m_foundTestScriptObject = null;

            TestSuite.TraverseTestTree(this, new TestSuite.TraverseTestTreeDelegate(searchTree), systemId);

            return m_foundTestScriptObject;
        }

        private bool searchTree(TestScriptObject testScriptObject, object tag)
        {
            bool @continue = true;

            if (testScriptObject.SystemID == (Guid)tag)
            {
                m_foundTestScriptObject = testScriptObject;
                @continue = false;
            }

            return @continue;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Class internal methods

        public TestScriptObject AddTestScriptObject(TestScriptObject testScriptObject)
        {
            return AddTestScriptObject(testScriptObject, -1);
        }

        public TestScriptObject AddTestScriptObject(TestScriptObject testScriptObject, int index)
        {
            testScriptObject.SetParent(this);
            _testScriptObjects.Insert(index == -1 ? _testScriptObjects.Count : index, testScriptObject);
            return testScriptObject;
        }

        public bool RemoveTestScriptObject(TestScriptObject testScriptObject)
        {
            return _testScriptObjects.Remove(testScriptObject);
        }

        public int FindTestScriptObjectIndex(TestScriptObject testScriptObject)
        {
            return this.TestScriptObjects.FindIndex(x => x.SystemID == testScriptObject.SystemID);
        }

        #endregion

        #region Class protected methods

        protected void AddRuntimeTestPropertiesToTestProperties()
        {
            foreach (TestProperty testProperty in TestProperties)
            {
                Core.TestProperties.SetPropertyValue(testProperty.Name, testProperty.Value);
            }
        }

        #endregion

        #region Class private methods

        protected List<TestScriptObject> getActiveChildren()
        {
            return _testScriptObjects.FindAll(x => x.Status == Core.Status.Active);
        }

        #endregion
    }
}
