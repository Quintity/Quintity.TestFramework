using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestCheckFailedException : TestException
    {
        #region Class data members

        [DataMember]
        private TestCheck _testCheck;

        [IgnoreDataMember]
        public TestCheck TestCheck
        {get { return this._testCheck; } }

        #endregion

        #region Class constructors

        /// <summary>
        /// Exception thrown when a TestClass-derived test check fails and test check exception flag is set to true.
        /// </summary>
        /// <param name="testCheck"></param>
        public TestCheckFailedException(TestCheck testCheck)
        {
            this._testCheck = testCheck;
        }

        #endregion
    }
}
