using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestPropertiesNotInitializedException : ApplicationException
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
