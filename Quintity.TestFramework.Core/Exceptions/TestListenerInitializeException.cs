using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestListenerInitializeException : Exception
    {
        public TestListenerInitializeException()
        {
        }

        public TestListenerInitializeException(string message)
            : base(message)
        {
        }

        public TestListenerInitializeException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public TestListenerInitializeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
