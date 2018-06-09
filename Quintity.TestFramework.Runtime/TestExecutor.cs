﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Runtime
{
    public partial class TestExecutor
    {
        #region Class delegate and event declarations with helper functions.

        //public delegate bool TraverseTestTreeDelegate(TestScriptObject testScriptObject, object tag);

        //public static void TraverseTestTree(TestScriptObject testScriptObject, TraverseTestTreeDelegate traverseTestTreeDelegate)
        //{
        //    TraverseTestTree(testScriptObject, traverseTestTreeDelegate, null);
        //}
        
        //public static void TraverseTestTree(TestScriptObject testScriptObject, TraverseTestTreeDelegate traverseTestTreeDelegate, object tag)
        //{
        //    m_continue = traverseTestTreeDelegate(testScriptObject, tag);

        //    if (m_continue)
        //    {
        //        if (testScriptObject is TestScriptObjectContainer)
        //        {
        //            var container = testScriptObject as TestScriptObjectContainer;

        //            foreach (TestScriptObject testscript in container.TestScriptObjects)
        //            {
        //                TraverseTestTree(testscript, traverseTestTreeDelegate, tag);

        //                if (!m_continue)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        public delegate void ExecutionBeginEventHandler(TestExecutor testExecutor, TestExecutionBeginArgs args);
        public static event ExecutionBeginEventHandler OnExecutionBegin;

        internal static void fireExecutionBeginEvent(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            OnExecutionBegin?.Invoke(testExecutor, args);
        }

        public delegate void ExecutionCompleteEventHandler(TestExecutor testExecutor, TestExecutionCompleteArgs args);
        public static event ExecutionCompleteEventHandler OnExecutionComplete;

        internal static void fireExecutionCompleteEvent(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            OnExecutionComplete?.Invoke(testExecutor, args);
        }

        public delegate void TestExecutionFinalizedEvent();
        public static event TestExecutionFinalizedEvent OnTestExecutionFinalizedEvent;

        internal static void fireTestExecutionFinalizedEvent()
        {
            OnTestExecutionFinalizedEvent?.Invoke();
        }

        #endregion

        #region Class data members

        private class ExecutionParameters
        {
            public string _virtualUser;
            public TestScriptObject _testScriptObject;
            public List<TestCase> _testCases;
            public TestProfile _testProfile;
            public bool _suppressExecution;
        }

        private static readonly log4net.ILog LogEvent = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //TODO - remove? static bool m_continue;  
        static internal bool SuppressExecution = false;
        //TODO:  05/26/2018 Need to fix up when resolving test listerner service.
        private ListenersService.ListenerEventsClient _listenerServiceClient = null;
        private TestScriptObject _initialTestScriptObject;
        private Timer _executionTimer = null;
        private Thread _mainExecutionThread;
        private TestProfile _testProfile = new TestProfile();
        private TestListenerCollection _testListeners;
        private int _virtualUsers;
        private List<Thread> _virtualUserThreads;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private bool _testListenersComplete = true;
        private Stopwatch _stopWatch;

        public TestScriptObject TestScriptObject
        {
            get { return _initialTestScriptObject; }
        }

        #endregion

        #region Class constructors

        public TestExecutor()
        {
            TestClassBase.OnStopExecutionRequest += TestClassBase_OnStopExecutionRequest;
            TestClassBase.OnPauseExecutionRequest += TestClassBase_OnPauseExecutionRequest;
            TestClassBase.OnContinueExecutionRequest += TestClassBase_OnContinueExecutionRequest;

            _virtualUserThreads = new List<Thread>();
        }

        private void TestClassBase_OnContinueExecutionRequest(TestClassBase testClass)
        { }

        private void TestClassBase_OnPauseExecutionRequest(TestClassBase testClass)
        { }

        #endregion

        #region Class public methods

        public void ExecuteTestSuite(TestSuite testSuite, List<TestCase> discreteTestCases, TestProfile testProfile, TestListenerCollection testListeners,
            bool suppressExecution)
        {
            LogEvent.Debug("Beginning execution");

            try
            {
                _testProfile = testProfile ?? new TestProfile();

                if (testListeners != null)
                {
                    LogEvent.Debug("Fixing up listeners");

                    _testListeners = fixupTestListeners(testListeners);

                    //TODO:  Fix up
                    //_listenerServiceClient = new ListenersService.Client.ListenerClient();

                    //var qualifiedListeners = _listenerServiceClient.Initialize(_testListeners, _testProfile);

                    //if (qualifiedListeners != 0)
                    //{
                    //    _testListenersComplete = false;

                    //    ListenersService.Client.ListenerClient.OnTestListenersComplete += ListenerClient_OnTestListenersComplete;
                    //}
                    //else
                    //{
                    //    _listenerServiceClient = null;

                    //    LogEvent.Info("The provided testlisteners configure did not contain any active or qualifying testlisteners");
                    //}
                }

                ExecutionParameters executionParameters = new ExecutionParameters()
                {
                    _testScriptObject = testSuite,
                    _testCases = discreteTestCases,
                    _testProfile = testProfile,
                    _suppressExecution = suppressExecution
                };

                LogEvent.Debug("Beginning executeTestScriptObject");

                executeTestScriptObject(executionParameters);
            }
            catch (Exception e)
            {
                LogEvent.Error(e.Message, e);
            }
        }

        private TestListenerCollection fixupTestListeners(TestListenerCollection testListeners)
        {
            var activeListeners = testListeners.FindAll(x => x.Status == Status.Active && x.Parameters != null);

            foreach(var activeListener in activeListeners)
            {
                var newParameters = new Dictionary<string, string>();

                foreach (var parameter in activeListener.Parameters)
                {
                    var key = parameter.Key;
                    var expandedValue = TestProperties.ExpandString(parameter.Value);

                    newParameters.Add(key, expandedValue);
                }

                activeListener.Parameters = newParameters;
            }

            return new TestListenerCollection(activeListeners) ;
        }

        private void ListenerClient_OnTestListenersComplete(TestListenerCollection testListeners, TestListenersCompleteArgs args)
        {
            _testListenersComplete = true;

            if(args.TerminationSource == TerminationReason.ListenerError)
            {
                StopExecution(null, args.TerminationSource, args.Explanation);
            }
            else if (_virtualUserRuntimeState.Count == 0)
            {
                fireTestExecutionFinalizedEvent();
            }
        }


        public void ExecuteTestCase(TestCase testCase, bool suppressExecution)
        {
            ExecutionParameters executionParameters = new ExecutionParameters()
            {
                _testScriptObject = testCase,
                _suppressExecution = suppressExecution,
                _testProfile = new TestProfile()
            };

            executeTestScriptObject(executionParameters);
        }

        public void ExecuteTestStep(TestStep testStep, bool suppressExecution)
        {
            ExecutionParameters executionParameters = new ExecutionParameters()
            {
                _testScriptObject = testStep,
                _suppressExecution = suppressExecution,
                _testProfile = new TestProfile()
            };

            executeTestScriptObject(executionParameters);
        }

        public void StopExecution(string virtualUser, TerminationReason terminationSource, string explanation = null)
        {
            _listenerServiceClient = null;
            _testListenersComplete = true;

            fireTestExecutionFinalizedEvent();

            // Kill off user execution threads, than main thread.
            if (_virtualUserThreads.Count > 0)
            {
                // Kill is virtual user thread.
                foreach (Thread virtualUserThread in _virtualUserThreads)
                {
                    if (virtualUserThread != null && virtualUserThread.IsAlive)
                    {
                        virtualUserThread.Abort();
                    }
                }

                // Now kill main thread
                _mainExecutionThread.Abort();
                _stopWatch.Stop();
                fireExecutionCompleteEvent(this,
                    new TestExecutionCompleteArgs(null, _initialTestScriptObject, terminationSource, _stopWatch.Elapsed, explanation));
            }
        }

        #endregion

        #region Class private methods

        /// <summary>
        /// Executes the test script object
        /// </summary>
        /// <param name="testScriptObject"></param>
        /// <param name="testCases"></param>
        private void executeTestScriptObject(ExecutionParameters executionParameters)
        {
            SuppressExecution = executionParameters._suppressExecution;

            _initialTestScriptObject = executionParameters._testScriptObject;

            // Must be a case or step...
            if (executionParameters._testProfile == null)
            {
                executionParameters._testProfile = new TestProfile(1, new TimeSpan(0, 0, 0), "VirtualUser");
            }

            _mainExecutionThread = new Thread(new ParameterizedThreadStart(executeOnMainThread));
            _mainExecutionThread.Name = "MainExecutionThread";
            _mainExecutionThread.SetApartmentState(ApartmentState.STA);
            _mainExecutionThread.IsBackground = true;
            _mainExecutionThread.Start(executionParameters);
        }

        private void executeOnMainThread(object executionParametersAsObject)
        {
            var executionParameters = executionParametersAsObject as ExecutionParameters;

            registerRuntimeEventHandlers();

            // Create virtual user status dictionary
            initializeVirtualUserStates(executionParameters);

            // Calculate wait time between virtual user launch.
            int wait =
                (int)(executionParameters._testProfile.TimeSpan.TotalMilliseconds / executionParameters._testProfile.VirtualUsers);

            // Initialize timer to launch users based on calculated wait time above.
            _executionTimer = new Timer(virtualUserExecutionCallBack, executionParameters, 0, wait);

            //Wait until signaled the execution is complete.
            _resetEvent.WaitOne();
        }

        private void initializeVirtualUserStates(ExecutionParameters executionParameters)
        {
            _virtualUserRuntimeState = new Dictionary<string, RuntimeStatus>();

            for (int i = 1; i <= _testProfile.VirtualUsers; i++)
            {
                var virtualUser = formatVirtualUserName(executionParameters, i);

                _virtualUserRuntimeState.Add(virtualUser, new RuntimeStatus());
            }
        }

        private void virtualUserExecutionCallBack(object stateInfo)
        {
            try
            {
                LogEvent.Debug("Starting virtualUserExecutionCallBack");

                var executionParameters = stateInfo as ExecutionParameters;

                // Increment virtual users spun up...
                _virtualUsers++;

                if (executionParameters._testProfile.VirtualUsers == _virtualUsers)
                {
                    _executionTimer?.Dispose();
                }

                var virtualUser = formatVirtualUserName(executionParameters, _virtualUsers);
                executionParameters._virtualUser = virtualUser;

                var virtualUserThread = new Thread(new ParameterizedThreadStart(executeOnVirtualUserThread));
                virtualUserThread.Name = virtualUser;

                virtualUserThread.SetApartmentState(ApartmentState.STA);
                virtualUserThread.IsBackground = true;
                Debug.WriteLine("Virtual user launched:  " + virtualUserThread.Name + "  " + DateTime.Now);
                virtualUserThread.Start(executionParameters);
                _virtualUserThreads.Add(virtualUserThread);
            }
            catch (Exception e)
            {
                LogEvent.Debug(e.ToString());
            }
            finally
            {
                LogEvent.Debug("Ending virtualUserExecutionCallBack");
            }
        }

        /// <summary>
        /// Generates virtual user name based on name specified in performance profile or 
        /// default of VirtualUser and id virtual user id (interation number).
        /// </summary>
        /// <param name="executionParameters"></param>
        /// <param name="virtualUserId"></param>
        /// <returns></returns>
        private string formatVirtualUserName(ExecutionParameters executionParameters, int virtualUserId)
        {
            string virtalUserName = null;

            if (executionParameters._testProfile.NameFormat != null)
            {
                virtalUserName = string.Format(executionParameters._testProfile.NameFormat, virtualUserId);
            }
            else
            {
                virtalUserName = "VirtualUser" + virtualUserId;
            }

            return virtalUserName;
        }

        private void executeOnVirtualUserThread(object executionParametersAsObject)
        {
            LogEvent.Debug("Starting executeOnVirtualUserThread");

            ExecutionParameters executionParameters = executionParametersAsObject as ExecutionParameters;

            TestScriptResult testScriptResult = null;

            // Fire of beginning execution event.
            fireExecutionBeginEvent(this, new TestExecutionBeginArgs(Thread.CurrentThread.Name, executionParameters._testScriptObject));

            _stopWatch = new Stopwatch();

            try
            {
                _stopWatch.Start();

                if (executionParameters._testScriptObject is TestSuite)
                {
                    testScriptResult = ((TestSuite)executionParameters._testScriptObject).Execute(executionParameters._testCases);
                }
                else if (executionParameters._testScriptObject is TestCase)
                {
                    testScriptResult = ((TestCase)executionParameters._testScriptObject).Execute();
                }
                else if (executionParameters._testScriptObject is TestStep)
                {
                    testScriptResult = ((TestStep)executionParameters._testScriptObject).Execute();
                }

                _stopWatch.Stop();

                // Fire execution complete event...
                fireExecutionCompleteEvent(this, new TestExecutionCompleteArgs(Thread.CurrentThread.Name, executionParameters._testScriptObject, 
                    TerminationReason.Normal, _stopWatch.Elapsed));
            }
            catch (ThreadAbortException)
            {
                // Eat this exception as is handled by StopExecution method.
            }
            catch (Exception e)
            {
                _stopWatch.Stop();
            
                fireExecutionCompleteEvent(this,
                    new TestExecutionCompleteArgs(Thread.CurrentThread.Name, _initialTestScriptObject, 
                    TerminationReason.RuntimeException, _stopWatch.Elapsed, e.ToString()));
            }
            finally
            {
            }
        }

        private void TestClassBase_OnStopExecutionRequest(TerminationReason terminationSource, string explanation)
        {
            StopExecution(null, terminationSource, explanation);
        }

        #endregion
    }
}