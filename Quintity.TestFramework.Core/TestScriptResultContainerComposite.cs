using System;
using System.Collections.Generic;

namespace Quintity.TestFramework.Core
{
    public class TestScriptResultContainerComposite : TestScriptResultComposite
    {
        #region Data members

        public string Author { get; }
        public DateTime Created { get; }
        public string FunctionalArea { get; }
        public TestPropertyCollection TestProperties { get; }
        public int DidNotExecute { get; }
        public int Errored { get; }
        public int Failed { get; }
        public int Inconclusive { get; }
        public int Passed { get; }
        public int Total { get; }

        internal List<TestScriptResultComposite> TestScripResultCollection = new List<TestScriptResultComposite>();

        #endregion

        #region Constructors

        public TestScriptResultContainerComposite(TestScriptObjectContainer testScriptObjectContainer,
            TestScriptResultContainer testScriptResultContainer) : base(testScriptObjectContainer, testScriptResultContainer)
        {
            Author = testScriptObjectContainer.Author;
            Created = testScriptObjectContainer.Created;
            FunctionalArea = testScriptObjectContainer.FunctionalArea;
            TestProperties = testScriptObjectContainer.TestProperties;

            DidNotExecute = testScriptResultContainer.DidNotExecute;
            Errored = testScriptResultContainer.Errored;
            Failed = testScriptResultContainer.Failed;
            Inconclusive = testScriptResultContainer.Inconclusive;
            Passed = testScriptResultContainer.Passed;
            Total = testScriptResultContainer.Total;
        }

        #endregion
    }
}
