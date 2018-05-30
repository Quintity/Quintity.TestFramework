using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Runtime
{
    [DataContract]
    abstract public class TestListener
    {
        #region Class data members

        public string VirtualUser
        { get; set; }

        #endregion

        #region Class constructors

        public TestListener()
        { }

        public TestListener(Dictionary<string, string> args)
        { }

        #endregion

        #region Class public methods

        public abstract void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args);
        public abstract void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args);
        public abstract void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args);
        public abstract void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args);
        public abstract void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult);
        public abstract void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args);
        public abstract void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult);
        public abstract void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult);
        public abstract void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args);
        public abstract void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult);

        public abstract void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args);
        public abstract void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult);
        public abstract void OnTestCheck(TestCheck testCheck);
        public abstract void OnTestWarning(TestWarning testWarning);
        public abstract void OnTestTrace(string virtualUser, string traceMessage);
        public abstract void OnTestMetric(string virtualUser, TestMetricEventArgs args);

        #endregion

        #region public public methods

        static public bool IsTestListenerType(Type type)
        {
            bool isClass = false;

            bool isListener = type.IsSubclassOf(typeof(TestListener));

            if (type.IsSubclassOf(typeof(TestListener)) &&
                type.IsVisible && type.IsPublic && !type.IsAbstract)
            {
                isClass = true;
            }

            return isClass;
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
