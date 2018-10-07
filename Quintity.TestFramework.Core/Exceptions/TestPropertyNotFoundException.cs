using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    public class TestPropertyNotFoundException : TestException
    {
        public TestPropertyNotFoundException()
        {
        }

        public TestPropertyNotFoundException(string message)
            : base(message)
        {
        }

        public TestPropertyNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public TestPropertyNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

