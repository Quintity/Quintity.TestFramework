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
    public class TestMetric : IDisposable
    {
        #region Data members

        private bool _isRunning;

        public string VirtualUser
        { get { return _virtualUser; } }

        private string _virtualUser;

        public string PerfID
        { get { return _perfId; } }

        private string _perfId;

        public string Description
        { get { return _description; } }

        private string _description;

        public List<string> StateArgs
        { get { return _stateArgs; } }

        private List<string> _stateArgs;

        public DateTime StartTime
        { get { return _startTime; } }

        private DateTime _startTime;

        public DateTime StopTime
        { get { return _stopTime; } }

        private DateTime _stopTime;

        public TimeSpan ElapsedTime
        { get { return _elapsedTime; } }

        private TimeSpan _elapsedTime;

        #endregion

        #region Events definition

        /// <summary>
        /// Test metric message event delegate definition.
        /// </summary>
        /// <param name="message">Test metric message.</param>
        public delegate void TestMetricEventHandler(string virtualUser, TestMetricEventArgs args);

        /// <summary>
        /// Test trace message event definition.
        /// </summary>
        public static event TestMetricEventHandler OnTestMetric;

        private void fireTestMetricEvent()
        {
            if (OnTestMetric != null)
            {
                var args = new TestMetricEventArgs(
                    _virtualUser, _perfId, _description, _startTime, _stopTime, _elapsedTime, _stateArgs);
                OnTestMetric(_virtualUser, args);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor of the TestMetric object.
        /// </summary>
        /// <param name="testClass">The test class containing the test metric request (typically "this").</param>
        /// <param name="perfId">A unique identifier to be used across all virtual users and test runs.  Once
        /// initially set for first run, if changed between runs, the history linkage to earlier run performance
        ///  metrics will be lost.</param>
        /// <param name="description">A mutable description of the desired metric.</param>
        /// <remarks>The performance timer does not automatically start, a subsequent call class Start is
        /// needed to start the timer.
        /// class Start method </remarks>
        public TestMetric(string perfId, string description)
            : this(perfId, false, description)
        { }

        /// <summary>
        /// Constructor of the TestMetric object.
        /// </summary>
        /// <param name="testClass">The test class containing the test metric request.</param>
        /// <param name="perfId">A unique identifier to be used across all virtual users and test runs.  Once
        /// initially set for first run, if changed between runs, the history linkage to earlier run performance
        ///  metrics will be lost.</param>
        /// <param name="start">True to immediate start the performance timer, false to set subsequently.</param>
        /// <param name="description">A mutable description of the desired metric.</param>
        public TestMetric(string perfId, bool start, string description)
        {
            _virtualUser = Thread.CurrentThread.Name;
            _perfId = perfId;
            _description = description;
            _stateArgs = new List<string>();

            if (start)
            {
                Start();
            }
        }

        #endregion

        #region public methods

        public void Dispose()
        {
            if (_isRunning)
            {
                Stop();
            }

            fireTestMetricEvent();
        }

        public void Start()
        {
            _isRunning = true;
            _startTime = DateTime.Now;
        }

        public void Stop()
        {
            _stopTime = DateTime.Now;
            _elapsedTime = _stopTime - _startTime;
            _isRunning = false;
        }

        #endregion
    }
}
