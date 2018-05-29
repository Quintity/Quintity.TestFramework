using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestScriptObjectExecutionCompleteArgs
    {
        #region Class data members

        [DataMember]
        protected TestScriptResult _testResult;

        [IgnoreDataMember]
        public TestScriptResult TestResult
        { get { return _testResult; } }

        #endregion

        #region Class constructors

        public TestScriptObjectExecutionCompleteArgs()
        { }

        public TestScriptObjectExecutionCompleteArgs(TestScriptResult testResult)
        {
            _testResult = testResult;
        }

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
