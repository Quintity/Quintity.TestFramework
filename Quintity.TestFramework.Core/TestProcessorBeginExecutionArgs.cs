using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestProcessorBeginExecutionArgs : TestScriptObjectBeginExecutionArgs
    {
        public TestProcessorBeginExecutionArgs(string virtualUser)
            : base(virtualUser)
        { }
    }
}
