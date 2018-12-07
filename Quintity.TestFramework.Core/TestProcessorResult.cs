using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestProcessorResult : TestStepResult
    {
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
