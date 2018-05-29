using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [CollectionDataContract]
    public class TestPropertyCollection2 : List<TestProperty>
    {
        #region Class constructors

        public TestPropertyCollection2()
            : base()
        {
        }

        public TestPropertyCollection2(TestPropertyCollection2 testPropertyCollection)
            : base(testPropertyCollection)
        {
        }

        #endregion
        #region Public methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TestPropertyCollection:\r\n{");

            foreach(TestProperty property in this)
            {
                sb.AppendLine(property.ToString());
                sb.AppendLine(null);
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        #endregion
    }
}
