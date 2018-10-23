using System;

namespace Quintity.TestFramework.Core
{
    public class TestScriptResultComposite
    {
        #region Data members

        public Guid ParentID { get; }
        public string Title { get; }
        public string Description { get; }
        public Status Status { get; }
        public string UserID { get; }

        public string VirtualUser { get; }
        public TestVerdict TestVerdict { get; }
        public string TestMessage { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan ElapsedTime { get; }

        #endregion

        #region Constructors

        public TestScriptResultComposite(TestScriptObject testScriptObject, TestScriptResult testScriptResult)
        {
            TestAssert.IsTrue(testScriptObject.SystemID.CompareTo(testScriptResult.ReferenceID) == 0,
                "Test test script result is not applicable to the specified script object.");

            ParentID = testScriptObject.ParentID;
            UserID = testScriptObject.UserID;
            Title = testScriptObject.Title;
            Description = testScriptObject.Description;
            Status = testScriptObject.Status;

            VirtualUser = testScriptResult.VirtualUser;
            TestVerdict = testScriptResult.TestVerdict;
            TestMessage = testScriptResult.TestMessage;
            StartTime = testScriptResult.StartTime;
            EndTime = testScriptResult.EndTime;
            ElapsedTime = testScriptResult.ElapsedTime;
        }

        #endregion
    }
}
