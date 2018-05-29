using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using System;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    [TypeConverter(typeof(TestExpandableObjectConverter))]
    public class TestPreprocessor : TestProcessor
    {
        #region Class events
        #endregion

        #region Class data members

        [DataMember(Order = 20)]
        private OnFailure _onFailure;

        [DefaultValue(OnFailure.Stop)]
        public OnFailure OnFailure
        {
            get { return _onFailure; }
            set
            {
                if (_onFailure != value)
                {
                    object oldValue = _onFailure;
                    _onFailure = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        #endregion

        #region Class constructors

        internal TestPreprocessor()
            : base()
        { }

        public TestPreprocessor(TestSuite parent)
            : base(parent)
        {
            _onFailure = OnFailure.Stop;
        }

        public TestPreprocessor(TestPreprocessor originalTestPreProcessor)
            : base(originalTestPreProcessor)
        {
            _onFailure = originalTestPreProcessor._onFailure;
        }

        public TestPreprocessor(TestSuite parent, XPathNavigator navigator)
           : base(parent, navigator)
        {
            Enum.TryParse(TestUtils.GetXPathValue(navigator, "OnFailure"), out _onFailure);
        }

        #endregion

        #region Class public members

        internal override TestProcessorResult Execute()
        {
            return base.Execute();
        }

        #endregion

        #region Class internal members

        override internal void Write(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("TestPreprocessor");

            base.Write(xmlWriter);

            xmlWriter.WriteElementString("OnFailure", _onFailure.ToString());

            xmlWriter.WriteEndElement();
        }

        #endregion
    }
}
