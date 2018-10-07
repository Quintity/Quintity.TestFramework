using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Used to indicate failure for a test. 
    /// </summary>
    [DataContract]
    public class TestAssertFailedException : TestException
    {
        /// <summary>
        /// Initializes a new instance of the AssertFailedExcepton class.
        /// </summary>
        /// <remarks>This uses a system supplied error message.</remarks>
        public TestAssertFailedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the AssertFailedExcepton class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TestAssertFailedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AssertFailedException class with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <remarks>An exception that is thrown as a direct result of a previous exception should include a reference to the 
        /// previous exception in the InnerException property. The InnerException property returns 
        /// the same value that is passed into the constructor, or null if the InnerException 
        /// property does not supply the inner exception value to the constructor.</remarks>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="exception">The exception that is the cause of the current exception. 
        /// If the ex parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public TestAssertFailedException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public TestAssertFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
