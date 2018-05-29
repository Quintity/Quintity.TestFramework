using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    abstract public class TestScriptResultContainer : TestScriptResult
    {
        #region Class data members

        [DataMember(Order = 10)]
        protected int _passed;

        [DataMember(Order = 11)]
        protected int _failed;

        [DataMember(Order = 12)]
        protected int _errored;

        [DataMember(Order = 13)]
        protected int _inconclusive;

        [DataMember(Order = 14)]
        protected int _didnotexecute;

        [DataMember(Order = 15)]
        protected TestScriptResultCollection _testScriptResults;

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public int Passed
        { get { return _passed; } }

        [IgnoreDataMember]
        public int Failed
        { get { return _failed; } }

        [IgnoreDataMember]
        public int Errored
        { get { return _errored; } }

        [IgnoreDataMember]
        public int Inconclusive
        { get { return _inconclusive; } }

        [IgnoreDataMember]
        public int DidNotExecute
        { get { return _didnotexecute; } }

        [IgnoreDataMember]
        public int Total
        { get { return _passed + _failed + _errored + _inconclusive + _didnotexecute; } }

        [IgnoreDataMember]
        public string FormattedCounters
        {
            get
            {
                return string.Format(
                    "Passed:  {0}, Failed:  {1}, Errored:  {2}, Inconclusive:  {3}, Did not execute:  {4}, Total:  {5}",
                    _passed, _failed, _errored, _inconclusive, _didnotexecute,
                    _passed + _failed + _errored + _inconclusive + _didnotexecute);
            }
        }

        #endregion

        #region Class constructors

        public TestScriptResultContainer()
        {
            _testScriptResults = new TestScriptResultCollection();
        }

        #endregion

        #region Class public methods

        internal void AddResult(TestScriptResult testScriptResult)
        {
            _testScriptResults.Add(testScriptResult);
        }

        public override string ToString()
        {
            return string.Format(
                "Passed:  {0}, Failed:  {1}, Errored:  {2}, Inconclusive:  {3}, Did not execute:  {4}, Total:  {5}\r\n\r\n{6}",
                _passed, _failed, _errored, _inconclusive, _didnotexecute,
                _passed + _failed + _errored + _inconclusive + _didnotexecute,
                base.ToString());
        }

        #endregion

        #region Class internal methods

        internal void IncrementCounter(TestVerdict testVerdict)
        {
            switch (testVerdict)
            {
                case TestVerdict.Pass:
                    _passed++;
                    break;
                case TestVerdict.Fail:
                    _failed++;
                    break;
                case TestVerdict.Error:
                    _errored++;
                    break;
                case TestVerdict.Inconclusive:
                    _inconclusive++;
                    break;
                case TestVerdict.DidNotExecute:
                    _didnotexecute++;
                    break;
                default:
                    break;
            }
        }

        internal void IncrementCounters(TestSuiteResult testSuiteResult)
        {
            _passed += testSuiteResult._passed;
            _failed += testSuiteResult._failed;
            _errored += testSuiteResult._errored;
            _inconclusive += testSuiteResult._inconclusive;
            _didnotexecute += testSuiteResult._didnotexecute;
        }

        internal void FinalizeVerdict()
        {
            if (_testVerdict == Core.TestVerdict.Unknown)
            {
                _testVerdict = Core.TestVerdict.Pass;

                if (_failed != 0 || _errored != 0)
                {
                    _testVerdict = TestVerdict.Fail;
                }
                else if (_inconclusive != 0)
                {
                    _testVerdict = TestVerdict.Inconclusive;
                }
            }
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
