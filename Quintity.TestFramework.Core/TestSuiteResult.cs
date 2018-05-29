using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestSuiteResult : TestScriptResultContainer
    {
        #region Class data members

        [DataMember]
        private TestProcessorResult _preProcessorResult;

        [IgnoreDataMember]
        public TestProcessorResult TestPrePocessorResult
        { get { return _preProcessorResult; } }

        [DataMember]
        private TestProcessorResult _postProcessorResult;

        [IgnoreDataMember]
        public TestProcessorResult TestPostPocessorResult
        { get { return _postProcessorResult; } }

        [IgnoreDataMember]
        public TestScriptResultCollection TestScriptResults
        {
            get { return _testScriptResults; }
        }

        //public override TestVerdict TestVerdict
        //{
        //    get
        //    {
        //        TestVerdict verdict = base.TestVerdict;

        //        if (_preProcessorResult != null)
        //        {
        //            verdict = updateTestVerdict(verdict, _preProcessorResult.TestVerdict);
        //        }

        //        if (_postProcessorResult != null)
        //        {
        //            verdict = updateTestVerdict(verdict, _preProcessorResult.TestVerdict);
        //        }

        //        return verdict;
        //    }
        //}

        #endregion

        #region Class constructors

        public TestSuiteResult()
        { }

        #endregion

        #region Class public methods

        public static void SerializeToFile(TestSuiteResult testSuiteResult, string filePath)
        {
            TestArtifact.SerializeToFile(testSuiteResult, filePath);
        }

        public static TestSuiteResult DeserializeFromFile(string filePath)
        {
            return TestArtifact.DeserializeFromFile(typeof(TestSuiteResult), filePath) as TestSuiteResult;
        }

        public override string ToString()
        {
            return string.Format("Test verdict:  {0}\r\nTest case summary:  {1}", TestVerdict, base.ToString());
        }

        #endregion

        #region Class internal methods

        internal void SetTestPreprocessorResult(TestProcessorResult result)
        {
            _preProcessorResult = result;
        }

        internal void SetTestPostprocessorResult(TestProcessorResult result)
        {
            _postProcessorResult = result;
        }

        #endregion

        #region Class private methods

        private TestVerdict updateTestVerdict(TestVerdict currentVerdict, TestVerdict processorVerdict)
        {
            switch (currentVerdict)
            {
                case TestVerdict.Unknown:
                    currentVerdict = processorVerdict;
                    break;

                case TestVerdict.DidNotExecute:
                    currentVerdict = processorVerdict;
                    break;

                case TestVerdict.Error:
                    break;

                case TestVerdict.Fail:
                    if (processorVerdict == Core.TestVerdict.Error)
                    { currentVerdict = processorVerdict; }
                    break;

                case TestVerdict.Inconclusive:
                    if (processorVerdict == Core.TestVerdict.Fail || processorVerdict == Core.TestVerdict.Error)
                    { currentVerdict = processorVerdict; }
                    break;
                case TestVerdict.Pass:
                    if (processorVerdict == Core.TestVerdict.Fail || processorVerdict == Core.TestVerdict.Error)
                    { currentVerdict = processorVerdict; }
                    break;
                default:
                    break;
            }

            return currentVerdict;
        }

        #endregion
    }
}
