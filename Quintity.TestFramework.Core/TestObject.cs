using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    abstract public class TestObject
    {
        #region Class data members

        #endregion

        #region Class constructors

        public TestObject(TestObject originalTestObject)
            : this()
        { }

        public TestObject()
        { }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
