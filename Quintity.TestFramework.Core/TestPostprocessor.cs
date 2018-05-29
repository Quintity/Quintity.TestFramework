using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    [TypeConverter(typeof(TestExpandableObjectConverter))]
    public class TestPostprocessor : TestProcessor
    {
        #region Class events
        #endregion

        #region Class data members
        #endregion

        #region Class constructors

        internal TestPostprocessor()
            : base()
        { }

        public TestPostprocessor(TestSuite parent)
            : base(parent)
        { }

        public TestPostprocessor(TestPostprocessor originalTestPostprocessor)
            : base(originalTestPostprocessor)
        { }

        public TestPostprocessor(TestSuite parent, XPathNavigator navigator)
            : base(parent, navigator)
        { }

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
            xmlWriter.WriteStartElement("TestPostprocessor");

            base.Write(xmlWriter);

            xmlWriter.WriteEndElement();
        }

        #endregion
    }
}
