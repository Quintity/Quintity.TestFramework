using System;
using System.Xml;
using System.Xml.XPath;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    sealed public class TestCase : TestScriptObjectContainer
    {
        #region Class events

        public delegate void ExecutionBeginEventHandler(TestCase testCase, TestCaseBeginExecutionArgs args);
        public static event ExecutionBeginEventHandler OnExecutionBegin;

        private void FireExecutionBeginEvent(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            OnExecutionBegin?.Invoke(testCase, args);
        }

        public delegate void ExecutionCompleteEventHandler(TestCase testCase, TestCaseResult testCaseResult);
        public static event ExecutionCompleteEventHandler OnExecutionComplete;

        private void FireExecutionCompleteEvent(TestCase testCase, TestCaseResult testCaseResult)
        {
            if (OnExecutionComplete != null)
            {
                OnExecutionComplete(testCase, testCaseResult);
            }
        }

        #endregion

        #region Class data members

        private Dictionary<int, TestClassBase> _testClassDictionary;

        [IgnoreDataMember]
        [BrowsableAttribute(false)]
        public TestScriptObjectCollection TestSteps
        { get { return _testScriptObjects; } }

        [DataMember(Order = 17)]
        private string _tags;

        [CategoryAttribute("General"),
        DescriptionAttribute("Comma delimited collection of tags."),
        PropertyOrder(41)]
        [Editor(typeof(TestTextBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Tags
        {
            get { return _tags; }
            set
            {
                if (_tags != value)
                {
                    object oldValue = _tags;
                    _tags = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 18)]
        private bool _knownDefects;

        [CategoryAttribute("General"),
        DisplayName("Known Defect"),
        DescriptionAttribute("True if test result is a known failure. false otherwise."),
        PropertyOrder(42),
        DefaultValue(false)]
        public bool KnownDefects
        {
            get { return _knownDefects; }
            set
            {
                if (_knownDefects != value)
                {
                    object oldValue = _knownDefects;
                    _knownDefects = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 19)]
        private string _defects;

        [CategoryAttribute("General"),
        DisplayName("Defects"),
        DescriptionAttribute("Identifier(s) of known related defects."),
        PropertyOrder(43)]
        [Editor(typeof(TestTextBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Defects
        {
            get { return _defects; }
            set
            {
                if (_defects != value)
                {
                    object oldValue = _defects;
                    _defects = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 20)]
        private OnFailure _onTestStepFailure;

        [CategoryAttribute("Runtime Settings"),
        DisplayName("On Step Failure"),
        DescriptionAttribute("True to continue execution of subsequent test steps following a step failure, false otherwise."),
        DefaultValue(OnFailure.Stop)]
        public OnFailure OnTestStepFailure
        {
            get { return _onTestStepFailure; }
            set
            {
                if (_onTestStepFailure != value)
                {
                    object oldValue = _onTestStepFailure;
                    _onTestStepFailure = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }


        #endregion

        #region Class constructors

        public TestCase()
            : this(null)
        {
            OnTestStepFailure = OnFailure.Stop;
        }

        public TestCase(string title)
            : this(title, null)
        { }

        public TestCase(string title, TestSuite parent)
            : base(title, parent)
        {
            _onTestStepFailure = OnFailure.Stop;
        }

        public TestCase(TestCase originalTestCase, TestSuite parent)
            : base(originalTestCase, parent)
        {
            _knownDefects = originalTestCase._knownDefects;
            _defects = TestUtils.SafeCopyString(originalTestCase._defects);
            _onTestStepFailure = originalTestCase._onTestStepFailure;
            _tags = TestUtils.SafeCopyString(originalTestCase._tags);
        }

        public TestCase(TestSuite parent, XPathNavigator nav)
            : base(null, parent)
        {
            Read(parent, nav);
        }

        #endregion

        #region Class public methods


        public TestStep AddTestStep(string title, int index = -1)
        {
            return AddTestStep(new TestStep(title), index);
        }

        public TestStep AddTestStep(TestStep testStep, int index = -1)
        {
            return AddTestScriptObject(testStep, index) as TestStep;
        }

        public bool RemoveTestStep(TestStep testStep)
        {
            return RemoveTestScriptObject(testStep);
        }

        public void ClearTestSteps()
        {
            _testScriptObjects.Clear();
        }

        public int FindTestStepIndex(TestStep testStep)
        {
            return FindTestScriptObjectIndex(testStep);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static void SerializeToFile(TestCase testCase, string filePath)
        {
            TestArtifact.SerializeToFile(testCase, filePath);
        }

        public static TestCase DeserializeFromFile(string filePath)
        {
            return TestArtifact.DeserializeFromFile(typeof(TestCase), filePath) as TestCase;
        }

        #endregion

        #region Class internal methods

        public TestCaseResult Execute()
        {
            string currentUser = Thread.CurrentThread.Name;
            FireExecutionBeginEvent(this, new TestCaseBeginExecutionArgs(currentUser));

            TestCaseResult result = new TestCaseResult();
            result.SetReferenceID(SystemID);
            result.SetVirtualUser(currentUser);

            // Save copy of test property collection for restoration later (maintains scope)
            TestPropertyCollection savedTestPropertyCollection = new TestPropertyCollection(Core.TestProperties.TestPropertyCollection);

            AddRuntimeTestPropertiesToTestProperties();

            _testClassDictionary = new Dictionary<int, TestClassBase>();

            List<TestScriptObject> activeTestSteps = getActiveChildren();

            result.SetStartTime(DateTime.Now);

            if (activeTestSteps.Count > 0)
            {
                // Iterate through each test step.
                foreach (TestStep testStep in activeTestSteps)
                {
                    // Only execute active test steps.
                    if (testStep.Status == Core.Status.Active)
                    {
                        // Stop if test case already failed (i.e., a previous test step has failed...UNLESS
                        // a) the test case is marked to continue on failure (continue regardless of previous failures.
                        // b) a specific test step is marked to always execute (regardless of previous failures).
                        if ((result.TestVerdict != TestVerdict.Fail || OnTestStepFailure == OnFailure.Continue) || testStep.AlwaysExecute)
                        {
                            var testStepResult = testStep.Execute(_testClassDictionary);

                            result.AddResult(testStepResult);
                            result.IncrementCounter(testStepResult.TestVerdict);

                            if (testStepResult.TestVerdict != TestVerdict.Pass)
                            {
                                result.FinalizeVerdict();
                            }
                        }
                        else
                        {
                            var testStepResult = new TestStepResult(TestVerdict.DidNotExecute,
                                "Did not execute as a previous step failed the test case." +
                                "  To always run, consider setting the \"Always Run\" property to true.");
                            result.AddResult(testStepResult);
                            result.IncrementCounter(testStepResult.TestVerdict);
                            testStep.FireExecutionCompleteEvent(testStep, testStepResult);
                        }
                    }
                }
            }
            else
            {
                result.SetTestVerdict(TestVerdict.DidNotExecute);
                result.SetTestMessage("The test case is set to \"Active\", however it does not contain active test steps.");
            }

            result.SetEndTime(DateTime.Now);
            result.FinalizeVerdict();

            // Restore preserved test property collection.
            Core.TestProperties.SetTestPropertyCollection(savedTestPropertyCollection);

            FireExecutionCompleteEvent(this, result);

            return result;
        }

        public void Write(XmlTextWriter xmlWriter)
        {
            // Write test case attributes.
            xmlWriter.WriteStartElement("TestScriptObject");
            xmlWriter.WriteAttributeString("type", "TestCase");
            xmlWriter.WriteAttributeString("status", _status.ToString());

            // Write test case elements.
            xmlWriter.WriteElementString("SystemID", _systemId.ToString());
            //xmlWriter.WriteElementString("ParentID", _parentId.ToString());
            xmlWriter.WriteElementString("UserID", "", _userId);
            xmlWriter.WriteElementString("Title", "", _title);
            xmlWriter.WriteElementString("Author", "", _author);
            xmlWriter.WriteElementString("Created", "", _created.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            xmlWriter.WriteElementString("FunctionalArea", _functionalArea);
            xmlWriter.WriteElementString("Tags", "", _tags);
            xmlWriter.WriteElementString("Defects", _defects);
            xmlWriter.WriteElementString("KnownDefects", _knownDefects.ToString());
            xmlWriter.WriteElementString("Reference", _reference);
            xmlWriter.WriteElementString("OnFailure", _onTestStepFailure.ToString());

            // Write description
            xmlWriter.WriteStartElement("Description");
            xmlWriter.WriteCData(Description);
            xmlWriter.WriteEndElement();

            // Write test case properties.
            Core.TestProperties.Write(xmlWriter, _testProperties);

            // Write test steps
            xmlWriter.WriteStartElement("TestScriptObjects");

            // If test actions exist, iterate through and write....
            if (TestSteps != null)
            {
                foreach (TestStep testStep in TestSteps)
                {
                    testStep.Write(xmlWriter);
                }
            }

            xmlWriter.WriteEndElement();  // TestSteps
            xmlWriter.WriteEndElement();  // TestScriptObject
        }

        public void Read(TestSuite testSuite, XPathNavigator nav)
        {
            // Parent ID is set at load time (i.e., not static)
            _parentID = testSuite.SystemID;

            Enum.TryParse(nav.GetAttribute("status", ""), out _status);
            _systemId = new Guid(TestUtils.GetXPathValue(nav, "SystemID"));
            _userId = TestUtils.GetXPathValue(nav, "UserID");
            _title = TestUtils.GetXPathValue(nav, "Title");
            _author = TestUtils.GetXPathValue(nav, "Author");
            _created = Convert.ToDateTime(TestUtils.GetXPathValue(nav, "Created"));
            _functionalArea = TestUtils.GetXPathValue(nav, "FunctionalArea");
            _tags = TestUtils.GetXPathValue(nav, "Tags");
            _defects = TestUtils.GetXPathValue(nav, "Defects");
            bool.TryParse(TestUtils.GetXPathValue(nav, "KnownDefects"), out _knownDefects);
            _reference = TestUtils.GetXPathValue(nav, "Reference");
            Enum.TryParse(TestUtils.GetXPathValue(nav, "OnFailure"), out _onTestStepFailure);
            _description = TestUtils.GetXPathValue(nav, "Description");


            XPathNodeIterator iterator = null;

            // Extract suite properties.
            //iterator = nav.Select("Properties");
            //iterator.MoveNext();
            //ReadProperties(iterator.Current);

            // Collect test steps.
            iterator = nav.Select("TestScriptObjects/TestScriptObject");

            // Iterate through test steps.
            while (iterator.MoveNext())
            {
                TestScriptObjects.Add(new TestStep(this, iterator.Current));
            }
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
