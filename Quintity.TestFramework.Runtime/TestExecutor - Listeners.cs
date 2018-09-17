using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime.ListenersService;

namespace Quintity.TestFramework.Runtime
{
    public partial class TestExecutor
    {
        #region Partial class data members

        private Dictionary<string, RuntimeStatus> _virtualUserRuntimeState;

        internal struct ParamsStruct
        {
            public TestListenerDescriptor TestListenerDescriptor
            { get; set; }

            public string Method
            { get; set; }

            public object[] Parameters
            { get; set; }
        }

        #endregion

        #region Partial class private methods

        internal class TestListenerEventParams
        {
            private string _virtualUser;
            public string VirtualUser
            { get { return _virtualUser; } }

            private string _method;
            public string Method
            { get { return _method; } }

            private object[] _parameters;
            public object[] Parameters
            { get { return _parameters; } }

            public TestListenerDescriptor TestListenerDescriptor
            { get; set; }

            public TestListenerEventParams(string virtualUser, string method, params object[] parameters)
            {
                _virtualUser = virtualUser;
                _method = method;
                _parameters = parameters;
            }
        }

        private class RuntimeStatus
        {
            public Queue<TestListenerEventParams> ListenerEventQueue { get; set; }

            public RuntimeStatus()
            {
                ListenerEventQueue = new Queue<TestListenerEventParams>();
            }
        }

        private void registerRuntimeEventHandlers()
        {
            // TestExecutor events
            TestExecutor.OnExecutionBegin += TestExecutor_OnExecutionBegin;
            TestExecutor.OnExecutionComplete += TestExecutor_OnExecutionComplete;

            // TestSuite events
            TestSuite.OnExecutionBegin += TestSuite_OnExecutionBegin;
            TestSuite.OnTestPreprocessorBegin += TestSuite_OnTestPreprocessorBegin;
            TestSuite.OnTestPreprocessorComplete += TestSuite_OnTestPreprocessorComplete;
            TestSuite.OnTestPostprocessorBegin += TestSuite_OnTestPostprocessorBegin;
            TestSuite.OnTestPostprocessorComplete += TestSuite_OnTestPostprocessorComplete;
            TestSuite.OnExecutionComplete += TestSuite_OnExecutionComplete;

            // TestCase events
            TestCase.OnExecutionBegin += TestCase_OnExecutionBegin;
            TestCase.OnExecutionComplete += TestCase_OnExecutionComplete;

            // TestStep events
            TestStep.OnExecutionBegin += TestStep_OnExecutionBegin;
            TestStep.OnExecutionComplete += TestStep_OnExecutionComplete;

            // Runtime events
            TestCheck.OnTestCheck += TestCheck_OnTestCheck;
            TestWarning.OnTestWarning += TestWarning_OnTestWarning;
            TestTrace.OnTestTrace += TestTrace_OnTestTrace;
            TestMetric.OnTestMetric += TestMetric_OnTestMetric;
        }

        private void unregisterRuntimeEventHandlers()
        {
            // TestExecutor events
            TestExecutor.OnExecutionBegin -= TestExecutor_OnExecutionBegin;
            TestExecutor.OnExecutionComplete -= TestExecutor_OnExecutionComplete;

            // TestSuite events
            TestSuite.OnExecutionBegin -= TestSuite_OnExecutionBegin;
            TestSuite.OnTestPreprocessorBegin -= TestSuite_OnTestPreprocessorBegin;
            TestSuite.OnTestPreprocessorComplete -= TestSuite_OnTestPreprocessorComplete;
            TestSuite.OnTestPostprocessorBegin -= TestSuite_OnTestPostprocessorBegin;
            TestSuite.OnTestPostprocessorComplete -= TestSuite_OnTestPostprocessorComplete;
            TestSuite.OnExecutionComplete -= TestSuite_OnExecutionComplete;

            // TestCase events
            TestCase.OnExecutionBegin -= TestCase_OnExecutionBegin;
            TestCase.OnExecutionComplete -= TestCase_OnExecutionComplete;

            // TestStep events
            TestStep.OnExecutionBegin -= TestStep_OnExecutionBegin;
            TestStep.OnExecutionComplete -= TestStep_OnExecutionComplete;

            // Runtime events
            TestCheck.OnTestCheck -= TestCheck_OnTestCheck;
            TestWarning.OnTestWarning -= TestWarning_OnTestWarning;
            TestTrace.OnTestTrace -= TestTrace_OnTestTrace;
            TestMetric.OnTestMetric -= TestMetric_OnTestMetric;
        }

        #region Runtime event handlers

        /// <summary>
        /// OnTestExecutionFinalized event handler
        /// </summary>
        private void TestExecutor_OnTestExecutionFinalizedEvent()
        {
            // Signal/release main execution thread.
            _resetEvent.Set();

            Debug.WriteLine("Unregistering runtime event handlers.");
            unregisterRuntimeEventHandlers();

            // Remove currently executing TestScriptObjects from TestProperties (no long meaningful).
            TestProperties.RemoveProperty("CurrentTestSuite");
            TestProperties.RemoveProperty("CurrentTestCase");
            TestProperties.RemoveProperty("CurrentTestStep");
        }

        private void TestExecutor_OnExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            _listenerEventsClient?.OnTestExecutionBegin(args);
        }

        private void TestExecutor_OnExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            _listenerEventsClient?.OnTestExecutionComplete(args);

            Thread.Sleep(3000);

            var available = _listenerEventsClient?.ServiceAvailability();


            _virtualUserRuntimeState.Remove(args.VirtualUser);

            if (_virtualUserRuntimeState.Count <= 0)
            {
                finalizeTestExecution();
            }
        }
        
        private void finalizeTestExecution()
        {
            //Debug.WriteLine("Unregistering runtime event handlers.");
            unregisterRuntimeEventHandlers();

            // Remove currently executing TestScriptObjects from TestProperties (no longer meaningful).
            TestProperties.RemoveProperty("CurrentTestSuite");
            TestProperties.RemoveProperty("CurrentTestCase");
            TestProperties.RemoveProperty("CurrentTestStep");

            fireTestExecutionFinalizedEvent();

            // Signal/release main execution thread.
            _resetEvent.Set();
        }

        private void TestSuite_OnExecutionBegin(TestScriptObject testScriptObject, TestSuiteBeginExecutionArgs args)
        {
            TestProperties.SetPropertyValue("CurrentTestSuite", testScriptObject);

            _listenerEventsClient?.OnTestSuiteExecutionBegin((TestSuite)testScriptObject, args);
        }

        private void TestSuite_OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            _listenerEventsClient?.OnTestPreprocessorBegin(testSuite, args);
        }
        
        private void TestSuite_OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            _listenerEventsClient?.OnTestPreprocessorComplete(testSuite, testProcessorResult);
        }

        private void TestSuite_OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            _listenerEventsClient?.OnTestPostprocessorBegin(testSuite, args);
        }

        private void TestSuite_OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            _listenerEventsClient?.OnTestPostprocessorComplete(testSuite, testProcessorResult);
        }

        private void TestSuite_OnExecutionComplete(TestScriptObject testScriptObject, TestScriptResult testScriptResult)
        {
            _listenerEventsClient?.OnTestSuiteExecutionComplete((TestSuite)testScriptObject, (TestSuiteResult)testScriptResult);
        }

        private void TestCase_OnExecutionBegin(TestScriptObject testScriptObject, TestCaseBeginExecutionArgs args)
        {
            TestProperties.SetPropertyValue("CurrentTestCase", testScriptObject);

            _listenerEventsClient?.OnTestCaseExecutionBegin((TestCase)testScriptObject, args);
        }

        private void TestCase_OnExecutionComplete(TestScriptObject testScriptObject, TestScriptResult testScriptResult)
        {
            _listenerEventsClient?.OnTestCaseExecutionComplete((TestCase)testScriptObject, (TestCaseResult)testScriptResult);
        }

        private void TestStep_OnExecutionBegin(TestScriptObject testScriptObject, TestStepBeginExecutionArgs args)
        {
            TestProperties.SetPropertyValue("CurrentTestStep", testScriptObject);

            _listenerEventsClient?.OnTestStepExecutionBegin((TestStep)testScriptObject, args);
        }

        private void TestStep_OnExecutionComplete(TestScriptObject testScriptObject, TestScriptResult testScriptResult)
        {
            _listenerEventsClient?.OnTestStepExecutionComplete((TestStep)testScriptObject, (TestStepResult)testScriptResult);
        }

        private void TestCheck_OnTestCheck(TestCheck testCheck)
        {
            _listenerEventsClient?.OnTestCheck(testCheck);
        }

        private void TestWarning_OnTestWarning(TestWarning testWarning)
        {
            _listenerEventsClient?.OnTestWarning(testWarning);
        }

        private void TestTrace_OnTestTrace(string virtualUser, string traceMessage)
        {
            _listenerEventsClient?.OnTestTrace(virtualUser, traceMessage);
        }

        private void TestMetric_OnTestMetric(string virtualUser, TestMetricEventArgs args)
        {
            _listenerEventsClient?.OnTestMetric(virtualUser, args);
        }

        #endregion

        #endregion
    }
}
