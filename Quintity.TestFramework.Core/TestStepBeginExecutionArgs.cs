using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestStepBeginExecutionArgs : TestScriptObjectBeginExecutionArgs
    {
        public TestStepBeginExecutionArgs(string virtualUser):base(virtualUser)
        { }
    }
}
