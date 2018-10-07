using System;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Verifies conditions in tests using true/false propositions.
    /// </summary>
    /// <remarks>This class contains a set of static methods that evaluate a Boolean condition. If this condition evaluates to true, the assertion passes.
    /// An assertion verifies an assumption of truth for compared conditions. The Assert class provides many static methods for verifying
    /// suppositions of truth. If the condition being verified is not true, the assertion fails.
    /// </remarks>
    public class TestAssert
    {
        #region Fail Assertions

        /// <summary>
        /// Fails the assertion without checking any conditions.
        /// </summary>
        public static void Fail()
        {
            throw new TestAssertFailedException();
        }

        public static void Fail(string format, params object[] args)
        {
            Fail(string.Format(format, args));
        }

        /// <summary>
        /// Fails the assertion without checking any conditions.
        /// </summary>
        /// <param name="message">A message to pass with the assertion.</param>
        public static void Fail(string message)
        {
            throw new TestAssertFailedException(message);
        }

        #endregion

        #region IsFalse Assertions

        /// <summary>
        /// Verifies that the specified condition is false. The assertion fails if the condition is true.
        /// </summary>
        /// <param name="condition">The condition to verify is false.</param>
        public static void IsFalse(Boolean condition)
        {
            // If true, throw an exception.
            if (condition)
            {
                throw new TestAssertFailedException();
            }
        }

        public static void IsFalse(Boolean condition, string format, params object[] args)
        {
            IsFalse(condition, string.Format(format, args));
        }

        /// <summary>
        /// Fails the assertion without checking any conditions.
        /// </summary>
        /// <param name="condition">The condition to verify is false.</param>
        /// <param name="message">A message to pass with the assertion.</param>
        public static void IsFalse(Boolean condition, string message)
        {
            // If true, throw an exception.
            if (condition)
            {
                throw new TestAssertFailedException(message);
            }
        }

        #endregion

        #region IsTrue Assertions

        /// <summary>
        /// Verifies that the specified condition is true. The assertion fails if the condition is false.
        /// </summary>
        /// <param name="condition">The condition to verify is true.</param>
        public static void IsTrue(Boolean condition)
        {
            if (!condition)
            {
                throw new TestAssertFailedException();
            }
        }

        public static void IsTrue(Boolean condition, string format, params object[] args)
        {
            IsTrue(condition, string.Format(format, args));
        }

        /// <summary>
        /// Verifies that the specified condition is true. The assertion fails if the condition is false.
        /// </summary>
        /// <param name="condition">The condition to verify is true.</param>
        /// <param name="message">A message to pass with the assertion.</param>
        public static void IsTrue(Boolean condition, string message)
        {
            if (!condition)
            {
                throw new TestAssertFailedException(message);
            }
        }

        #endregion

        #region IsNotNull Assertions

        /// <summary>
        /// Verifies that the specified object is not null. The assertion fails if it is null
        /// </summary>
        /// <param name="value">The object to verify is not null.</param>
        public static void IsNotNull(Object value)
        {
            // If value is null, throw exception.
            if (null == value)
            {
                throw new TestAssertFailedException();
            }
        }

        public static void IsNotNull(Object value, string format, params object[] args)
        {
            IsNotNull(value, string.Format(format, args));
        }

        /// <summary>
        /// Verifies that the specified object is not null. The assertion fails if it is null
        /// </summary>
        /// <param name="value">The object to verify is not null.</param>
        /// <param name="message">A message to pass with the assertion.</param>
        public static void IsNotNull(Object value, string message)
        {
            // If value is null, throw exception.
            if (null == value)
            {
                throw new TestAssertFailedException(message);
            }
        }

        #endregion

        #region IsNull Assertions

        /// <summary>
        /// Verifies that the specified object is null. The assertion fails if it is not null.
        /// </summary>
        /// <param name="value">The object to verify is null.</param>
        public static void IsNull(Object value)
        {
            // If value is null, throw exception.
            if (null != value)
            {
                throw new TestAssertFailedException();
            }
        }

        public static void IsNull(Object value, string format, params object[] args)
        {
            IsNull(value, string.Format(format, args));
        }

        /// <summary>
        /// Verifies that the specified object is not null. The assertion fails if it is not null
        /// </summary>
        /// <param name="value">The object to verify is null.</param>
        /// <param name="message">A message to pass with the assertion.</param>
        public static void IsNull(Object value, string message)
        {
            // If value is null, throw exception.
            if (null != value)
            {
                throw new TestAssertFailedException(message);
            }
        }

        #endregion
    }
}
