using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.TestProject
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        [TestMethod]
        public TestVerdict ScratchTest()
        {
            return TestVerdict;
        }
    }
}
