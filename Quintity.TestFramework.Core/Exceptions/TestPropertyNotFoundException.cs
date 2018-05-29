using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestPropertyNotFoundException : ApplicationException
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

