using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    abstract public class TestScriptResult : TestArtifact
    {
        #region Class data members

        [DataMember(Order = 1)]
        protected Guid _referenceID;

        [DataMember(Order = 2)]
        protected TestVerdict _testVerdict;

        [DataMember(Order = 3)]
        protected string _testMessage;

        [DataMember(Order = 3)]
        protected string _virtualUser;

        [DataMember(Order = 4)]
        protected DateTime _startTime;

        [DataMember(Order = 5)]
        protected DateTime _endTime;

        #endregion
        
        #region Class properties

        [IgnoreDataMember]
        public Guid ReferenceID
        { get { return _referenceID; } }

        [IgnoreDataMember]
        public TestVerdict TestVerdict
        { get { return _testVerdict; } }

        [IgnoreDataMember]
        public string TestMessage
        { get { return _testMessage; } }

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        [IgnoreDataMember]
        public DateTime StartTime
        { get { return _startTime; } }

        [IgnoreDataMember]
        public DateTime EndTime
        { get { return _endTime; } }

        [IgnoreDataMember]
        public TimeSpan ElapsedTime
        { get { return _endTime - _startTime; } }

        #endregion

        #region Class constructors

        public TestScriptResult()
            : base()
        {
            _virtualUser = Thread.CurrentThread.Name;
        }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return string.Format("Test verdict:  {0}\r\nTest message:  {1}\r\n\r\nStart time:  {2}\r\nEnd time: {3}\r\nElapsed time:  {4}",
                _testVerdict,
                !string.IsNullOrEmpty(_testMessage) ? _testMessage : "None",
                _startTime,
                _endTime,
                _endTime - _startTime);
        }

        #endregion

        #region Class internal methods

        internal void SetReferenceID(Guid referenceID)
        {
            _referenceID = referenceID;
        }

        internal void SetTestVerdict(TestVerdict testVerdict)
        {
            _testVerdict = testVerdict;
        }

        internal void SetTestMessage(string testMessage)
        {
            _testMessage += testMessage;
        }

        internal void SetVirtualUser(string virtualUser)
        {
            //_virtualUser = virtualUser;
        }

        internal void SetStartTime(DateTime startTime)
        {
            _startTime = startTime;
        }

        internal void SetEndTime(DateTime endTime)
        {
            _endTime = endTime;
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
