using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    public class TestPropertyNameException : TestException
    {
        public TestPropertyNameException()
        {
        }

        public TestPropertyNameException(string message)
            : base(message)
        {
        }

        public TestPropertyNameException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public TestPropertyNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

