using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestClassBase
    {
        #region Class events

        public delegate void PauseExecutionEventHandler(TestClassBase testClass);
        public static event PauseExecutionEventHandler OnPauseExecutionRequest;

        private void fireOnPauseExecutionRequest()
        {
            if (OnPauseExecutionRequest != null)
            {
                OnPauseExecutionRequest(this);
            }
        }

        protected void PauseExecution()
        {
            fireOnPauseExecutionRequest();
        }

        public delegate void ContinueExecutionEventHandler(TestClassBase testClass);
        public static event ContinueExecutionEventHandler OnContinueExecutionRequest;

        private void fireOnContinueExecutionRequest()
        {
            if (OnContinueExecutionRequest != null)
            {
                OnContinueExecutionRequest(this);
            }
        }

        protected void ContinueExecution()
        {
            fireOnContinueExecutionRequest();
        }

        public delegate void StopExecutionRequestEventHandler(TerminationReason terminationSource, string explanation);
        public static event StopExecutionRequestEventHandler OnStopExecutionRequest;

        private void fireStopExecutionRequestEvent(string explanation = null)
        {
            if (OnStopExecutionRequest != null)
            {
                OnStopExecutionRequest(TerminationReason.RuntimeStopRequest, explanation);
            }
        }

        #endregion

        #region Class data members

        public string VirtualUser
        { get; set; }

        private const int _defaultMaxTestCheckFailures = 50;

        private TestCheckCollection _testCheckCollection;
        public TestCheckCollection TestChecks
        { get { return _testCheckCollection; } }

        private TestWarningCollection _testWarningCollection;
        public TestWarningCollection TestWarnings
        { get { return _testWarningCollection; } }

        internal TestDataCollection _testDataCollection;
        public TestDataCollection TestData
        { get { return _testDataCollection; } }

        private TestVerdict _testVerdict;
        public TestVerdict TestVerdict
        {
            get { return _testVerdict; }
            set
            {
                _testVerdict = value;
            }
        }

        public string TestMessage
        { get; set; }

        public int MaxFailedTestChecks
        { get; set; }

        public bool TestCheckFailuresOnly
        { get; set; }


        #endregion

        #region Class constructors

        public TestClassBase()
        {
            _testVerdict = Core.TestVerdict.Pass;

            _testCheckCollection = new TestCheckCollection();
            TestCheck.OnTestCheck += TestCheck_OnTestCheck;

            _testWarningCollection = new TestWarningCollection();
            TestWarning.OnTestWarning += TestWarning_OnTestWarning;

            _testDataCollection = new TestDataCollection();

            TestCheckFailuresOnly = true;
            MaxFailedTestChecks = _defaultMaxTestCheckFailures;
        }

        #endregion

        #region Class test methods

        [TestMethod()]
        virtual public TestVerdict DumpProperties()
        {
            try
            {
                this.Setup();

                TestMessage += TestProperties.ToString();
                TestVerdict = Core.TestVerdict.Pass;
               
            }
            catch (Exception e)
            {
                this.TestMessage += e.Message;
                this.TestVerdict = TestVerdict.Error;
            }
            finally
            {
                this.Teardown();
            }

            return TestVerdict;
        }

        /// <summary>
        /// Thread sleep for the disignated number of milliseconds.
        /// </summary>
        /// <param name="millisecondTimeout"></param>
        /// <returns></returns>
        [TestMethod()]
        virtual public TestVerdict Sleep(int millisecondTimeout )
        {
            try
            {
                this.Setup();

                Thread.Sleep(millisecondTimeout);

                TestVerdict = Core.TestVerdict.Pass;

            }
            catch (Exception e)
            {
                this.TestMessage += e.Message;
                this.TestVerdict = TestVerdict.Error;
            }
            finally
            {
                this.Teardown();
            }

            return TestVerdict;
        }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        internal void ResetTestCheckCollection()
        {
            if (_testCheckCollection == null)
            {
                _testCheckCollection = new TestCheckCollection();
            }
            else
            {
                _testCheckCollection.Clear();
            }
        }

        internal void ResetTestWarningCollection()
        {
            if (_testWarningCollection == null)
            {
                _testWarningCollection = new TestWarningCollection();
            }
            else
            {
                _testWarningCollection.Clear();
            }
        }

        internal void ResetTestDataCollection()
        {
            if (_testDataCollection == null)
            {
                _testDataCollection = new TestDataCollection();
            }
            else
            {
                _testDataCollection.Clear();
            }
        }

        #region Class protected methods

        protected string GetCurrentUser() => Thread.CurrentThread.Name;

        /// <summary>
        /// This will stop all current runtime execution.  
        /// </summary>
        /// <param name="explanation">Stop execution explanation</param>
        protected void StopExecution(string explanation)
        {
            fireStopExecutionRequestEvent(string.Format("{0}\r\nSource:  {1}", explanation, getSource()));
        }

        /// <summary>
        /// Default test method setup method.  Setting test message to null and test result to unknown.
        /// </summary>
        virtual protected void Setup()
        { }

        /// <summary>
        /// Default test method tear down.
        /// </summary>
        virtual protected void Teardown()
        { }

        /// <summary>
        /// Convenience method around test TestProperties ExpandString method.
        /// </summary>
        /// <param name="testProperty"></param>
        /// <returns>Expanded string or original string if not expandable</returns>
        protected string ExpandString(string testProperty)
        {
            return string.IsNullOrEmpty(testProperty) ? testProperty : TestProperties.ExpandString(testProperty);
        }

        #endregion

        #region Class private methods

        private string getSource()
        {
            StackFrame frame = new StackFrame(2, true);

            Type type = frame.GetMethod().DeclaringType;

            string assembly = type.Assembly.ManifestModule.Name;
            string declaringType = type.Name;
            string method = frame.GetMethod().Name;
            string sourceFile = frame.GetFileName();
            int lineNumber = frame.GetFileLineNumber();

            string format = "Assembly:  {0}, Class: {1}, Method:  {2}\r\n    Source:  {3} {4}";

            string source = string.Format(format,
                    assembly,
                    declaringType,
                    method,
                    sourceFile,
                    lineNumber != 0 ? " (" + lineNumber + ")" : null);
            return source;
        }

        private List<TestCheck> getFailedTestChecks()
        {
            return _testCheckCollection.FindAll(x => x.TestVerdict == Core.TestVerdict.Fail);
        }

        private void TestCheck_OnTestCheck(TestCheck testCheck)
        {
            if (TestCheckFailuresOnly)
            {
                if (testCheck.TestVerdict == Core.TestVerdict.Fail)
                {
                    _testCheckCollection.Add(testCheck);
                }
            }
            else
            {
                _testCheckCollection.Add(testCheck);
            }

            if (MaxFailedTestChecks != -1 && getFailedTestChecks().Count() >= MaxFailedTestChecks)
            {
                throw new MaxFailedTestChecksException(string.Format(
                    "The maximum number of failed test checks ({0}) has been met or exceeded.   " +
                    "Consider increasing the test class's MaxFailedTestChecks property value to a higher number " +
                    "setting it to -1 (disregards max threshold),",
                    MaxFailedTestChecks));
            }
        }

        void TestWarning_OnTestWarning(TestWarning testWarning)
        {
            _testWarningCollection.Add(testWarning);
        }


        #endregion
    }
}
