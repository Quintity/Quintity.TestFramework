using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestClassNotFoundException : Exception
    {
        public TestClassNotFoundException()
        {
        }

        public TestClassNotFoundException(string message)
            : base(message)
        {
        }

        public TestClassNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public TestClassNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
