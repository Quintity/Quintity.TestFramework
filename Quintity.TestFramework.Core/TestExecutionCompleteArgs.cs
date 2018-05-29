using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [KnownType(typeof(TestSuite))]
    [KnownType(typeof(TestCase))]
    [KnownType(typeof(TestStep))]
    [DataContract]
    public class TestExecutionCompleteArgs
    {
        #region Class data members

        [DataMember]
        private String _virtualUser;
        [DataMember]
        private TestScriptObject _testScriptObject;
        [DataMember]
        private TerminationReason _terminationSource;
        [DataMember]
        private string _explanation;
        [DataMember]
        private TimeSpan _elapsedTime;

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        [IgnoreDataMember]
        public TestScriptObject TestScriptObject
        { get { return _testScriptObject; } }

        [IgnoreDataMember]
        public TerminationReason TerminationSource
        { get { return _terminationSource; } }

        [IgnoreDataMember]
        public string Explanation
        { get { return _explanation; } }

        [IgnoreDataMember]
        public TimeSpan ElapsedTime
        { get { return _elapsedTime; } }

        #endregion

        #region Class constructors

        public TestExecutionCompleteArgs(string virtualUser, TestScriptObject testScriptObject,
            TerminationReason terminationSource, TimeSpan elapsedTime, string explanation = null)
        {
            _virtualUser = virtualUser;
            _testScriptObject = testScriptObject;
            _terminationSource = terminationSource;
            _explanation = explanation;
            _elapsedTime = elapsedTime;
        }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return $"Virtual user:  {_virtualUser}, Termination source:  {_terminationSource} \r\nExplanation:  {_explanation}\r\nElapsed time:  {_elapsedTime}";
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
