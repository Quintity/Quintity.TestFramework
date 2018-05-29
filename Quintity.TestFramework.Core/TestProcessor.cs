using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    abstract public class TestProcessor : TestScriptObject
    {
        #region Class data members

        // These overrides are to avoid displaying in PropertyDataGrid
        [Browsable(false)]
        public override Guid SystemID
        {
            get
            {
                return base.SystemID;
            }
        }

        [Browsable(false)]
        public override string ParentIDAsString
        {
            get
            {
                return base.ParentIDAsString;
            }
        }

        [Browsable(false)]
        public override string UserID
        {
            get
            {
                return base.UserID;
            }
            set
            {
                base.UserID = value;
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

        [DataMember(Order = 4)]
        private bool _ignoreResult;

        [DefaultValue(false),
        PropertyOrder(31)]
        public bool IgnoreResult
        {
            get { return _ignoreResult; }
            set
            {
                if (_ignoreResult != value)
                {
                    object oldValue = _ignoreResult;
                    _ignoreResult = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 5)]
        private TestAutomationDefinition _testAutomationDefinition;

        [DisplayName("Automation definition"),
        PropertyOrder(100)]
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

        internal TestProcessor()
            : base()
        { }

        public TestProcessor(TestProcessor originalTestProcessor)
        {
            _testAutomationDefinition = new TestAutomationDefinition(originalTestProcessor._testAutomationDefinition);
            _parentID = originalTestProcessor._parentID;
            _status = originalTestProcessor._status;
            _ignoreResult = originalTestProcessor._ignoreResult;
        }

        public TestProcessor(TestSuite parent)
            : this()
        {
            _parentID = parent.SystemID;

            _status = Status.Incomplete;
            _ignoreResult = false;

            _testAutomationDefinition = new Core.TestAutomationDefinition();
        }

        public TestProcessor(TestSuite parent, XPathNavigator navigator)
        {
            _parentID = parent.SystemID;

            _systemId = new Guid(TestUtils.GetXPathValue(navigator, "SystemID"));
            _title = TestUtils.GetXPathValue(navigator, "Title");

            _description = TestUtils.GetXPathValue(navigator, "Description");
            Enum.TryParse(TestUtils.GetXPathValue(navigator, "Status"), out _status);
            bool.TryParse(TestUtils.GetXPathValue(navigator, "IgnoreResult"), out _ignoreResult);
            Enum.TryParse(TestUtils.GetXPathValue(navigator, "TestType"), out _testType);

            XPathNodeIterator iterator = navigator.Select("TestAutomationDefinition");
            iterator.MoveNext();
            _testAutomationDefinition = new TestAutomationDefinition(iterator.Current); 
        }

        #endregion

        #region Class internal members

        virtual internal TestProcessorResult Execute()
        {
            TestProcessorResult result = new TestProcessorResult();

            TestAssert.IsTrue(TestAutomationDefinition.IsCompleted(),
                "The test suite's processor is \"Active\", however its automation definition is incomplete.");

            var resultStruct = TestAutomationDefinition.Invoke(null);

            result.SetReferenceID(SystemID);
            result.SetVirtualUser(Thread.CurrentThread.Name);
            result.SetStartTime(resultStruct.StartTime);
            result.SetEndTime(resultStruct.EndTime);
            result.SetTestVerdict(resultStruct.TestVerdict);
            result.SetTestMessage(resultStruct.TestMessage);
            result.SetTestChecks(resultStruct.TestChecks);
            result.SetTestWarnings(resultStruct.TestWarnings);

            return result;
        }

        virtual internal void Write(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteElementString("SystemID", _systemId.ToString());
            xmlWriter.WriteElementString("Title", _title);

            // Write description
            xmlWriter.WriteStartElement("Description");
            xmlWriter.WriteCData(_description);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("Description", _description);
            xmlWriter.WriteElementString("Status", _status.ToString());
            xmlWriter.WriteElementString("IgnoreResult", IgnoreResult.ToString());
            xmlWriter.WriteElementString("TestType", TestType.ToString());

            TestAutomationDefinition.Write(xmlWriter);
        }

        #endregion

        #region Class private members
        #endregion

    }
}
