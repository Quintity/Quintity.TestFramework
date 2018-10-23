using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Quintity.TestFramework.Core.Support
{
    public class TestWait<T> : ITestWait<T>
    {
        #region Data members

        private T input;
        private IClock clock;
        private TimeSpan timeout = DefaultSleepTimeout;
        private TimeSpan sleepInterval = DefaultSleepTimeout;
        private string message = string.Empty;
        private List<Type> ignoredExceptions = new List<Type>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="timeout">The timeout duration in milliseconds to test a condition.</param>
        public TestWait(T input)
            : this(input, string.Empty, DefaultSleepTimeout, DefaultSleepTimeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="timeout">The timeout duration in milliseconds to test a condition.</param>
        public TestWait(T input, double timeout)
            : this(input, string.Empty, TimeSpan.FromMilliseconds(timeout), DefaultSleepTimeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="timeout">The timeout duration to test a condition.</param>
        public TestWait(T input, TimeSpan timeout)
            : this(input, string.Empty, timeout, DefaultSleepTimeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="message">Message to added to timeout exception</param>
        /// <param name="timeout">The timeout duration in milliseconds to test a condition.</param>
        public TestWait(T input, string message, double timeout)
            : this(input, message, TimeSpan.FromMilliseconds(timeout), DefaultSleepTimeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="message">Message to added to timeout exception</param>
        /// <param name="timeout">The timeout duration in milliseconds to test a condition.</param>
        /// <param name="pollingInterval">The wait time in milliseconds between condition executions.</param>
        public TestWait(T input, string message, double timeout, double pollingInterval)
            : this(input, message, TimeSpan.FromMilliseconds(timeout), TimeSpan.FromMilliseconds(pollingInterval))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="message">Message to added to timeout exception</param>
        /// <param name="timeout">The timeout duration to test a condition.</param>
        public TestWait(T input, string message, TimeSpan timeout)
            : this(input, message, timeout, DefaultSleepTimeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="message">Message to added to timeout exception</param>
        /// <param name="timeout">The timeout duration to test a condition.</param>
        /// <param name="pollingInterval">The wait time between condition executions.</param>
        public TestWait(T input, string message, TimeSpan timeout, TimeSpan pollingInterval )
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "Input value cannot be null");
            }

            this.input = input;
            this.message = message;
            this.timeout = timeout;
            sleepInterval = pollingInterval;
            clock = new SystemClock();
        }

        #endregion

        #region Public members

        /// <summary>
        /// Gets or sets how long to wait for the evaluated condition to be true. The default timeout is 500 milliseconds.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// Gets or sets how often the condition should be evaluated. The default timeout is 500 milliseconds.
        /// </summary>
        public TimeSpan PollingInterval
        {
            get { return sleepInterval; }
            set { sleepInterval = value; }
        }

        /// <summary>
        /// Gets or sets the message to be displayed when time expires.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Configures this instance to ignore specific types of exceptions while waiting for a condition.
        /// Any exceptions not whitelisted will be allowed to propagate, terminating the wait.
        /// </summary>
        /// <param name="exceptionTypes">The types of exceptions to ignore.</param>
        public void IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            if (exceptionTypes == null)
            {
                throw new ArgumentNullException("exceptionTypes", "ExceptionTypes cannot be null");
            }

            foreach (Type exceptionType in exceptionTypes)
            {
                if (!typeof(Exception).IsAssignableFrom(exceptionType))
                {
                    throw new ArgumentException("All types to be ignored must derive from System.Exception", "exceptionTypes");
                }
            }

            this.ignoredExceptions.AddRange(exceptionTypes);
        }

        /// <summary>
        /// Repeatedly applies this instance's input value to the given function until one of the following
        /// occurs:
        /// <para>
        /// <list type="bullet">
        /// <item>the function returns neither null nor false</item>
        /// <item>the function throws an exception that is not in the list of ignored exception types</item>
        /// <item>the timeout expires</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The delegate's expected return type.</typeparam>
        /// <param name="condition">A delegate taking an object of type T as its parameter, and returning a TResult.</param>
        /// <returns>The delegate's return value.</returns>
        public TResult Until<TResult>(Func<T, TResult> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "Condition cannot be a null value");
            }

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException("Can only wait on an object or boolean response, tried to use type: " + resultType.ToString(), "condition");
            }

            Exception lastException = null;
            var endTime = this.clock.LaterBy(this.timeout);
            while (true)
            {
                try
                {
                    var result = condition(this.input);
                    if (resultType == typeof(bool))
                    {
                        var boolResult = result as bool?;
                        if (boolResult.HasValue && boolResult.Value)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!this.IsIgnoredException(ex))
                    {
                        throw;
                    }

                    lastException = ex;
                }

                // Check the timeout after evaluating the function to ensure conditions
                // with a zero timeout can succeed.
                if (!this.clock.IsNowBefore(endTime))
                {
                    string timeoutMessage = string.Format(CultureInfo.InvariantCulture, "TestWait timeout elapsed after {0} seconds.", this.timeout.TotalSeconds);

                    if (!string.IsNullOrEmpty(this.message))
                    {
                        timeoutMessage += ": " + this.message;
                    }

                    this.ThrowTimeoutException(timeoutMessage, lastException);
                }

                Thread.Sleep(this.sleepInterval);
            }
        }

        #endregion

        #region Protected and private members

        /// <summary>
        /// Throws a <see cref="TestWaitTimeoutException"/> with the given message.
        /// </summary>
        /// <param name="exceptionMessage">The message of the exception.</param>
        /// <param name="lastException">The last exception thrown by the condition.</param>
        /// <remarks>This method may be overridden to throw an exception that is
        /// idiomatic for a particular test infrastructure.</remarks>
        protected virtual void ThrowTimeoutException(string exceptionMessage, Exception lastException)
        {
            throw new TestWaitTimeoutException(exceptionMessage, lastException);
        }

        private bool IsIgnoredException(Exception exception)
        {
            return this.ignoredExceptions.Any(type => type.IsAssignableFrom(exception.GetType()));
        }

        private static TimeSpan DefaultSleepTimeout
        {
            get { return TimeSpan.FromMilliseconds(500); }
        }

        #endregion
    }

}
