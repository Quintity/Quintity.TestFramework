using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [KnownType(typeof(TestSuite))]
    [KnownType(typeof(TestCase))]
    [KnownType(typeof(TestStep))]
    [DataContract]
    public class TestExecutionBeginArgs
    {
        #region Class data members

        [DataMember]
        private string _virtualUser;

        [DataMember]
        private TestScriptObject _testScriptObject;

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        [IgnoreDataMember]
        public TestScriptObject TestScriptObject
        { get { return _testScriptObject; } }

        #endregion

        #region Class constructors

        public TestExecutionBeginArgs(string virtualUser, TestScriptObject testScriptObject)
        {
            _virtualUser = virtualUser;
            _testScriptObject = testScriptObject;
        }

        #endregion

        #region Class public methods

        #endregion

        #region Class private methods

        #endregion
    }
}
