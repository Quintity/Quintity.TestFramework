using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [CollectionDataContract
     (Name = "TestScriptObjects",
     ItemName = "TestScriptObject")]
    [KnownType(typeof(TestSuite))]
    [KnownType(typeof(TestCase))]
    [KnownType(typeof(TestStep))]
    public class TestScriptObjectCollection : List<TestScriptObject>
    {
        #region Class data members

        #endregion

        #region Class constructors

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Class private methods

        #endregion

        #region Class static methods

        #endregion
    }
}
