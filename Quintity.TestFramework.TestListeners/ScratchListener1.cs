using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using log4net;

namespace Quintity.TestFramework.TestListeners
{

    public class ScratchListener1 : TestListener
    {
        private static readonly log4net.ILog LogEvent = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, string> parameters;

        public ScratchListener1()
        {
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name}");
        }

        public ScratchListener1(Dictionary<string, string> args)
        {

            //LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name}");

            //parameters = args;

            //foreach (var parameter in args)
            //{
            //    LogEvent.Debug($"ScratchListener1 parameter key:  {parameter.Key}, value:  {parameter.Value}");
            //}
        }

        public override void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            LogEvent.Debug($"Listener instance virtual user:  {VirtualUser}");

            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time}");
        }

        public override void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {VirtualUser}, sleep time: {time}");

        }

        public override void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {

            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time}");

        }

        public override void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
            //throwException();

            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testCaseResult.VirtualUser}, sleep time: {time}");

        }

        private void throwException()
        {
            throw new InvalidOperationException();
        }

        public override void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time} seconds");

        }

        public override void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {testStepResult.VirtualUser}, sleep time: {time} seconds");
        }

        public override void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} {args.VirtualUser}, sleep time: {time} seconds");
        }

        public override void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            int time = 500;
            Thread.Sleep(time);
            LogEvent.Debug($"{MethodInfo.GetCurrentMethod().Name} Scratch:  {testSuiteResult.VirtualUser}, sleep time: {time}");

        }

        #region Not used

        public override void OnTestMetric(string virtualUser, TestMetricEventArgs args) { }

        public override void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) { }

        public override void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) { }

        public override void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args) { }

        public override void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult) { }

        public override void OnTestTrace(string virtualString, string traceMessage) { }

        public override void OnTestWarning(TestWarning testWarning) { }

        public override void OnTestCheck(TestCheck testCheck) { }

        #endregion
    }
}
