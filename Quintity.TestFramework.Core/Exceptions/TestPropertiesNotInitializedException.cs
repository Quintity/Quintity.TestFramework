using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    public class TestPropertiesNotInitializedException : TestException
    {
        public TestPropertiesNotInitializedException()
        {
        }

        public TestPropertiesNotInitializedException(string message)
            : base(message)
        {
        }

        public TestPropertiesNotInitializedException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public TestPropertiesNotInitializedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
