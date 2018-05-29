using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [CollectionDataContract]
    public class TestPropertyCollection : List<TestProperty>
    {
        #region Class constructors

        public TestPropertyCollection()
            : base()
        {
        }

        public TestPropertyCollection(TestPropertyCollection testPropertyCollection)
            : base(testPropertyCollection)
        {
        }

        #endregion
        #region Public methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TestPropertyCollection:\r\n{");

            foreach (TestProperty property in this)
            {
                sb.AppendLine(property.ToString());
                sb.AppendLine(null);
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        public bool TryGetValue(string name, out TestProperty testProperty)
        {
            bool success = false;

            testProperty = this.Where(i => i.Name.ToUpper() == name.ToUpper() && i.Active == true).FirstOrDefault();

            // The [Null] is property, it is ok to return a null.
            if (name.ToUpper().Equals("NULL") || null != testProperty)
            {
                success = true;
            }

            return success;
        }

        #endregion
    }
}
