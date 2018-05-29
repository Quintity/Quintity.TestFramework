using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestSuiteBeginExecutionArgs : TestScriptObjectBeginExecutionArgs
    {
        public TestSuiteBeginExecutionArgs(string virtualUser):base(virtualUser)
        { }
    }
}
