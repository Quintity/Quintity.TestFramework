using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestPropertyNameException : ApplicationException
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

