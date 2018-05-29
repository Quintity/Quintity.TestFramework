using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestSystemProperty : TestProperty
    {
        #region Data members

        #endregion

        #region Constructors

        public TestSystemProperty()
        { }

        public TestSystemProperty(string name, object value)
            : base(name, value)
        { }

        public TestSystemProperty(string name, string description, object value, bool active)
            : base(name, description, value, active)
        { }

        public TestSystemProperty(TestSystemProperty testSystemProperty)
            : base(testSystemProperty)
        { }

        #endregion
    }
}
