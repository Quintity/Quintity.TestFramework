using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Used to indicate failure for a test. 
    /// </summary>
    [DataContract]
    public class MaxFailedTestChecksException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the AssertFailedExcepton class.
        /// </summary>
        /// <remarks>This uses a system supplied error message.</remarks>
        //public MaxFailedTestChecksException()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the AssertFailedExcepton class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal MaxFailedTestChecksException(string message)
            : base(message)
        {
        }
    }
}
