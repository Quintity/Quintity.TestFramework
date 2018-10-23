using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestCaseResultComposite : TestScriptResultContainerComposite
    {

        public string Defects { get; }
        public bool KnownDefects { get; }
        public OnFailure OnTestStepFailure { get; }
        public string Tags { get; }

        public object TestStepResults
        { get { return TestScripResultCollection; } }

        #region Constructors

        public TestCaseResultComposite(TestCase testCase, TestCaseResult testCaseResult)
            : base(testCase, testCaseResult)
        {
            Defects = testCase.Defects;
            KnownDefects = testCase.KnownDefects;
            OnTestStepFailure = testCase.OnTestStepFailure;
            Tags = testCase.Tags;


            foreach (var testStepResult in testCaseResult.TestStepResults)
            {
                var testStep = testCase.TestSteps.Find(x => x.SystemID.CompareTo(testStepResult.ReferenceID) == 0);

                if (!(testStep is null))
                {
                    TestScripResultCollection.Add(new TestStepResultComposite(testStep as TestStep, testStepResult as TestStepResult));
                }
            }
        }

        #endregion
    }









}
