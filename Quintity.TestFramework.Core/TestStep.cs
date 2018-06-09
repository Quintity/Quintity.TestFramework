using System.Threading;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.XPath;
using System;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    [DefaultProperty("UserID")]
    sealed public class TestStep : TestScriptObject
    {
        #region Class events

        public delegate void ExecutionBeginEventHandler(TestStep testStep, TestStepBeginExecutionArgs args);
        public static event ExecutionBeginEventHandler OnExecutionBegin;

        private void FireExecutionBeginEvent(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            if (OnExecutionBegin != null)
            {
                OnExecutionBegin(testStep, args);
            }
        }

        public delegate void ExecutionCompleteEventHandler(TestStep testStep, TestStepResult testStepResult);
        public static event ExecutionCompleteEventHandler OnExecutionComplete;

        internal void FireExecutionCompleteEvent(TestStep testStep, TestStepResult testStepResult)
        {
            if (OnExecutionComplete != null)
            {
                OnExecutionComplete(testStep, testStepResult);
            }
        }

        #endregion

        #region Class data members

        [DataMember(Order = 19)]
        private string _expectedBehaviour;

        [BrowsableAttribute(false)]
        public string ExpectedBehavior
        {
            get { return _expectedBehaviour; }
            set
            {
                if (_expectedBehaviour != value)
                {
                    object oldValue = _expectedBehaviour;
                    _expectedBehaviour = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 20)]
        private TestType _testType;

        [Browsable(false)]
        public TestType TestType
        {
            get { return _testType; }
            set
            {
                if (_testType != value)
                {
                    object oldValue = _testType;
                    _testType = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 22)]
        private TestVerdict _expectedTestVerdict;

        [CategoryAttribute("General"),
        DisplayName("Expected"),
        DescriptionAttribute("Expected test verdict."),
        PropertyOrder(21),
        DefaultValue(TestVerdict.Pass)]
        public TestVerdict ExpectedTestVerdict
        {
            get { return _expectedTestVerdict; }
            set
            {
                if (_expectedTestVerdict != value)
                {
                    object oldValue = _expectedTestVerdict;
                    _expectedTestVerdict = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 22)]
        private bool _alwaysExecute;

        [CategoryAttribute("Runtime Settings"),
        DisplayName("Always Execute"),
        DescriptionAttribute("True to always execture, regardless of any previous failures, False otherwise."),
        PropertyOrder(22),
        DefaultValue(false)]
        public bool AlwaysExecute
        {
            get { return _alwaysExecute; }
            set
            {
                if (_alwaysExecute != value)
                {
                    object oldValue = _alwaysExecute;
                    _alwaysExecute = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 21)]
        [Browsable(false)]
        private int _iterations;

        [CategoryAttribute("Runtime Settings"),
        DescriptionAttribute("The number of test step iterations."),
        PropertyOrder(23),
        DefaultValueAttribute(1)]
        [Browsable(false)]  // For now until implementation is determined.
        public int Iterations
        {
            get { return _iterations; }
            set
            {
                if (_iterations != value)
                {
                    object oldValue = _iterations;
                    _iterations = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 23)]
        private TestAutomationDefinition _testAutomationDefinition;

        [Browsable(false)]
        public TestAutomationDefinition TestAutomationDefinition
        {
            get { return _testAutomationDefinition; }
            set
            {
                if (!_testAutomationDefinition.Equals(value))
                {
                    object oldValue = _testAutomationDefinition;
                    _testAutomationDefinition = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        #endregion

        #region Class constructors

        public TestStep()
            : this(string.Empty)
        { }

        public TestStep(string title)
            : this(title, null)
        { }

        public TestStep(string title, TestCase parent)
            : base(title, parent)
        {
            _testAutomationDefinition = new Core.TestAutomationDefinition();
            _testType = Core.TestType.Manual;
            _expectedTestVerdict = TestVerdict.Pass;
            _alwaysExecute = false;
            _status = Core.Status.Incomplete;
            _iterations = 1;
            TestScriptObject.OnPrepareToSave += TestScriptObject_OnPrepareToSave;
        }

        public TestStep(TestStep originalTestStep, TestCase parentTestCase)
            : base(originalTestStep, parentTestCase)
        {
            this._expectedBehaviour = originalTestStep._expectedBehaviour;
            this._testType = originalTestStep._testType;
            this._expectedTestVerdict = originalTestStep._expectedTestVerdict;
            this._alwaysExecute = originalTestStep._alwaysExecute;
            this._iterations = originalTestStep._iterations;
            this._testAutomationDefinition = new TestAutomationDefinition(originalTestStep.TestAutomationDefinition);
        }

        public TestStep(TestCase parent, XPathNavigator nav )
            : base(title: null, parent: parent)
        {
            Read(parent, nav);
        }

        void TestScriptObject_OnPrepareToSave(TestScriptObject testScriptObject)
        { }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Class internal methods
        public TestStepResult Execute()
        {
            return Execute(null);
        }

        internal TestStepResult Execute(Dictionary<int, TestClassBase> testClassDictionary)
        {
            TestAssert.IsNotNull(TestAutomationDefinition,
                "The test step is marked as active, however its automation definition has not been set.");

            string currentUser = Thread.CurrentThread.Name;

            TestStepResult result = new TestStepResult();
            result.SetReferenceID(SystemID);
            result.SetVirtualUser(currentUser);
            result.SetTestVerdict(TestVerdict.Pass);

            FireExecutionBeginEvent(this, new TestStepBeginExecutionArgs(currentUser));

            TestAutomationDefinition.ResultStruct resultStruct = TestAutomationDefinition.Invoke(testClassDictionary);

            result.SetStartTime(resultStruct.StartTime);
            result.SetEndTime(resultStruct.EndTime);
            result.SetTestVerdict(determineVerdict(ExpectedTestVerdict, resultStruct.TestVerdict));
            result.SetTestMessage(resultStruct.TestMessage);
            result.SetTestChecks(resultStruct.TestChecks);
            result.SetTestWarnings(resultStruct.TestWarnings);
            result.SetTestData(resultStruct.TestData);

            FireExecutionCompleteEvent(this, result);

            return result;
        }

        internal void Write(XmlTextWriter xmlWriter)
        {
            // Setup up test element container.
            xmlWriter.WriteStartElement("TestScriptObject");
            xmlWriter.WriteAttributeString("type", "TestStep");
            xmlWriter.WriteAttributeString("status", _status.ToString());
            xmlWriter.WriteAttributeString("testType", _testType.ToString());

            // Suite header information.
            xmlWriter.WriteElementString("SystemID", _systemId.ToString());
            //xmlWriter.WriteElementString("ParentID", _parentId.ToString());
            xmlWriter.WriteElementString("UserID", _userId);
            xmlWriter.WriteElementString("Title", "", _title);

            // Write automated test expected results (Pass/Fail)
            xmlWriter.WriteElementString("ExpectedTestVerdict", _expectedTestVerdict.ToString());
            xmlWriter.WriteElementString("AlwaysExecute", _alwaysExecute.ToString());

            // Currently unused
            xmlWriter.WriteElementString("Iterations", _iterations.ToString());

            // Write description
            xmlWriter.WriteStartElement("Description");
            xmlWriter.WriteCData(Description);
            xmlWriter.WriteEndElement();

            // Write expected
            xmlWriter.WriteStartElement("ExpectedBehaviour");
            xmlWriter.WriteCData(_expectedBehaviour);
            xmlWriter.WriteEndElement();

            // Write automation definition.
            TestAutomationDefinition.Write(xmlWriter);

            xmlWriter.WriteEndElement();  // TestScriptObject
        }

        internal void Read(TestCase parent, XPathNavigator nav)
        {
            _parentID = parent.SystemID;

            // Start parsing
            Enum.TryParse(nav.GetAttribute("status", ""), out _status);
            Enum.TryParse(nav.GetAttribute("testType", ""), out _testType);
            _systemId = new Guid(TestUtils.GetXPathValue(nav, "SystemID"));
            _userId = TestUtils.GetXPathValue(nav, "UserID");
            _title = TestUtils.GetXPathValue(nav, "Title");
            Enum.TryParse(TestUtils.GetXPathValue(nav, "ExpectedTestVerdict"), out _expectedTestVerdict);
            bool.TryParse(TestUtils.GetXPathValue(nav, "AlwaysExecute"), out _alwaysExecute);
            int.TryParse(TestUtils.GetXPathValue(nav, "Iterations"), out _iterations);
            _description = TestUtils.GetXPathValue(nav, "Description");
            _expectedBehaviour = TestUtils.GetXPathValue(nav, "ExpectedBehaviour");

            // Get test automation definition
            XPathNodeIterator iterator = nav.Select("TestAutomationDefinition");
            iterator.MoveNext();
            _testAutomationDefinition = new TestAutomationDefinition(iterator.Current);
        }

        private TestVerdict determineVerdict(TestVerdict expected, TestVerdict actual)
        {
            return expected.Equals(actual) ? TestVerdict.Pass : TestVerdict.Fail;
        }

        #endregion
    }
}
