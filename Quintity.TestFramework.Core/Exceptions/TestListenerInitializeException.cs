using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    public class TestListenerInitializeException : TestException
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
