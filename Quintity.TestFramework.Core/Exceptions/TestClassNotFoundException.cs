using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    public class TestClassNotFoundException : TestException
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
