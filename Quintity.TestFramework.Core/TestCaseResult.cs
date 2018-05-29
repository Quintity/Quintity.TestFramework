using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestCaseResult : TestScriptResultContainer
    {
        #region Class data members

        [IgnoreDataMember]
        public TestScriptResultCollection TestStepResults
        {
            get { return _testScriptResults; }
        }

        #endregion

        #region Class constructors

        public TestCaseResult()
        { }

        #endregion

        #region Class public methods

        public static void SerializeToFile(TestCaseResult testCaseResult, string filePath)
        {
            TestArtifact.SerializeToFile(testCaseResult, filePath);
        }

        public static TestCaseResult DeserializeFromFile(string filePath)
        {
            return TestArtifact.DeserializeFromFile(typeof(TestCaseResult), filePath) as TestCaseResult;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
