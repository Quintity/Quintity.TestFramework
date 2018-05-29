using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
  public class TestSystemPropertyChangeException : ApplicationException
    {
        public TestSystemPropertyChangeException()
        {
        }

        public TestSystemPropertyChangeException(string message)
            : base(message)
        {
        }

        public TestSystemPropertyChangeException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public TestSystemPropertyChangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

