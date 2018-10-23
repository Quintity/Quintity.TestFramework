using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    class TestStepResultComposite : TestScriptResultComposite
    {
        #region Data members

        public bool AlwaysExecute { get; }
        public string ExpectedBehavior { get; }
        public TestVerdict ExpectedTestVerdict { get; }
        public int Iterations { get; }
        public TestAutomationDefinition TestAutomationDefinition { get; }
        public TestType TestType { get; }
        public TestCheckCollection TestChecks { get; }
        public TestDataCollection TestData { get; }
        public TestWarningCollection TestWarnings { get; }

        #endregion

        #region Constructors

        public TestStepResultComposite(TestStep testStep, TestStepResult testStepResult) :
            base(testStep, testStepResult)
        {
            AlwaysExecute = testStep.AlwaysExecute;
            ExpectedBehavior = testStep.ExpectedBehavior;
            ExpectedTestVerdict = testStep.ExpectedTestVerdict;
            Iterations = testStep.Iterations;
            TestAutomationDefinition = testStep.TestAutomationDefinition;
            TestType = testStep.TestType;

            TestChecks = testStepResult.TestChecks;
            TestData = testStepResult.TestData;
            TestWarnings = testStepResult.TestWarnings;
        }

        #endregion
    }
}
