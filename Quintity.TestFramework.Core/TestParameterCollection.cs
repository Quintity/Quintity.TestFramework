using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.XPath;
using System.Xml;

namespace Quintity.TestFramework.Core
{
    [CollectionDataContract
    (Name = "TestParameters",
    ItemName = "TestParameter")]
    public class TestParameterCollection : List<TestParameter>
    {
        #region Class data members

        #endregion

        #region Class constructors

        public TestParameterCollection()
        { }

        public TestParameterCollection(IEnumerable<TestParameter> testParameters)
            : base(testParameters)
        { }

        #endregion

        #region Class public methods

        internal static TestParameterCollection ReadParameters(XPathNavigator navigator)
        {
            var testParameters = new TestParameterCollection();

            XPathNodeIterator iterator = navigator.Select("TestParameters/TestParameter");

            while (iterator.MoveNext())
            {
                testParameters.Add(TestParameter.ReadTestParameter(iterator.Current));
            }

            return testParameters;
        }

        internal void Write(XmlTextWriter xmlWriter)
        {
            foreach (TestParameter testParameter in this)
            {
                testParameter.Write(xmlWriter);
            }
        }

        public Type[] GetParameterTypes()
        {
            List<Type> types = new List<Type>();

            foreach (TestParameter parameter in this)
            {
                types.Add(parameter.GetType());
            }

            return types.ToArray<Type>();
        }

        public object[] GetParameterValues()
        {
            List<object> values = new List<object>();

            foreach (TestParameter parameter in this)
            {
                var @value  = parameter.GetValue();

                if (@value is string)
                {
                    @value = TestProperties.ExpandString(@value as string, true);
                }

                values.Add(@value);
            }

            return values.ToArray<object>();

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (this.Count() > 0)
            {
                sb.AppendLine("TestParameters:");

                int i = 0;

                foreach (TestParameter testParameter in this)
                {
                    i++;
                    sb.AppendLine(string.Format("  {0}. {1},", i, testParameter.ToString()));
                }
            }
            else
            {
                sb.AppendLine("TestParameters:  None");
            }

            return sb.ToString();
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
