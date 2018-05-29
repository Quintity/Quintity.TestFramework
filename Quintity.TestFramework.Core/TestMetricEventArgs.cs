using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestMetricEventArgs : TestArtifact
    {
        #region Data members

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        [DataMember]
        private string _virtualUser;

        [IgnoreDataMember]
        public string PerfID
        { get { return _perfId; } }

        [DataMember]
        private string _perfId;

        [IgnoreDataMember]
        public string Description
        { get { return _description; } }

        [DataMember]
        private string _description;

        [IgnoreDataMember]
        public List<string> StateArgs
        { get { return _stateArgs; } }

        [DataMember]
        private List<string> _stateArgs;

        [IgnoreDataMember]
        public DateTime StartTime
        { get { return _startTime; } }

        [DataMember]
        private DateTime _startTime;

        [IgnoreDataMember]
        public DateTime StopTime
        { get { return _stopTime; } }

        [DataMember]
        private DateTime _stopTime;

        [IgnoreDataMember]
        public TimeSpan ElapsedTime
        { get { return _elapsedTime; } }

        [DataMember]
        private TimeSpan _elapsedTime;

        #endregion

        #region Constructors

        internal TestMetricEventArgs(string virtualUser, string perfId, string description, DateTime startTime, DateTime stopTime,
            TimeSpan elapsedTime, List<string> stateArgs)
        {
            //_virtualUser = virtualUser;
            _virtualUser = Thread.CurrentThread.Name;
            _perfId = perfId;
            _description = description;
            _startTime = startTime;
            _stopTime = stopTime;
            _elapsedTime = elapsedTime;
            _stateArgs = stateArgs;
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            string format = "Virtual user:  {0}\r\nPer ID:  {1}\r\nDescription:  {2}\r\n" + 
                "Start time:{3}\r\nStop time:  {4}\r\nElapsed time:  {5}\r\nState args:  {6}";

            return string.Format(format,
                _virtualUser,
                _perfId,
                _description,
                _startTime,
                _stopTime,
                _elapsedTime,
                _stateArgs.Count() > 0 ? "\r\n" + formatStateArgs() : "None");
        }

        public string FlattenStateArgs()
        {
            return formatStateArgs();
        }

        #endregion

        #region Private methods

        private string formatStateArgs()
        {
            StringBuilder sb = new StringBuilder();

            foreach(string startArg in _stateArgs)
            {
                sb.AppendFormat("{0}, ", startArg);
            }

            return sb.ToString().TrimEnd(new char[] {',',' ' });
        }

        #endregion
    }
}
