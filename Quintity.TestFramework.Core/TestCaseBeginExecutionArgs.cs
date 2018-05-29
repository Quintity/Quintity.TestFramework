using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestCaseBeginExecutionArgs : TestScriptObjectBeginExecutionArgs
    {
        public TestCaseBeginExecutionArgs(string virtualUser):base(virtualUser)
        { }
    }
}
