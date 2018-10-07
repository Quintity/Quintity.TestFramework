using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Represents exceptions that are thrown when an error occurs during actions.
    /// </summary>
    [Serializable]
    public class TestWaitTimeoutException : TestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestWaitTimeoutException"/> class.
        /// </summary>
        public TestWaitTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWaitTimeoutException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TestWaitTimeoutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWaitTimeoutException"/> class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or <see langword="null"/> if no inner exception is specified.</param>
        public TestWaitTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWaitTimeoutException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual
        /// information about the source or destination.</param>
        protected TestWaitTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
