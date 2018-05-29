using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// The TestWarning object records the results of a inprocess test validation check.
    /// </summary>
    /// 
    [DataContract]
    public class TestWarning : TestArtifact
    {
        #region Event and delegate definitions

        public delegate void TestWarningEventHandler(TestWarning testWarning);
        public static event TestWarningEventHandler OnTestWarning;

        internal static void FireTestWarningEvent(TestWarning testWarning)
        {
            if (TestWarning.OnTestWarning != null)
            {
                TestWarning.OnTestWarning(testWarning);
            }
        }

        #endregion

        #region Data members
        private static readonly string messageFormat = "Expected:  {0}, Actual:  {1}";

        [DataMember(Order = 0)]
        private string _virtualUser;

        [DataMember(Order = 0)]
        private string _comment;		// Optional comment.

        [DataMember(Order = 1)]
        private string _source;

        #endregion

        #region Class constructors

        /// <summary>
        /// Constructor with name, verdict and comments for a runtime test check validation.
        /// </summary>
        /// <param name="description">Name of test check.</param>
        /// <param name="verdict">Verdict of test check.</param>
        /// <param name="comment">Optional comment of the test check.</param>
        internal TestWarning(string comment)
        {
            _virtualUser = Thread.CurrentThread.Name;
            _comment = comment;
            _source = getSource();
        }

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        /// <summary>
        /// Returns the optional comment associated with test check.
        /// </summary>
        [IgnoreDataMember]
        public string Comment
        { get { return _comment; } }

        [IgnoreDataMember]
        public string Source
        { get { return _source; } }

        #endregion properties

        #region Class public methods

        public static void Fail(string expected, string actual)
        {
            Fail(messageFormat, expected, actual);
        }

        public static void Fail(string format, params object[] args)
        {
            Fail(string.Format(format, args));
        }

        public static void Fail(string comment)
        {
            FireTestWarningEvent(new TestWarning(comment));
        }

        public static void Pass(string expected, string actual)
        {
            Pass(messageFormat, expected, actual);
        }

        public static void Pass(string format, params object[] args)
        {
            Fail(string.Format(format, args));
        }

        public static void Pass(string comment)
        {
            FireTestWarningEvent(new TestWarning(comment));
        }

        public static void IsFalse(bool condition, string expected, string actual)
        {
            IsFalse(condition, messageFormat, expected, actual);
        }

        public static void IsFalse(bool condition, string format, params object[] args)
        {
            IsFalse(condition, string.Format(format, args));
        }

        public static void IsFalse(bool condition, string comment)
        {
            if (condition)
            {
                FireTestWarningEvent(new TestWarning(comment));
            }
        }

        public static void IsTrue(bool condition, string expected, string actual)
        {
            IsTrue(condition, messageFormat, expected, actual);
        }

        public static void IsTrue(bool condition, string format, params object[] args)
        {
            IsTrue(condition, string.Format(format, args));
        }

        public static void IsTrue(bool condition, string comment)
        {
            if (!condition)
            {
                FireTestWarningEvent(new TestWarning(comment));
            }
        }

        public static void IsNull(object value, string expected, string actual)
        {
            IsNull(value, messageFormat, expected, actual);
        }

        public static void IsNull(object value, string format, params object[] args)
        {
            IsNull(value, string.Format(string.Format(format, args)));
        }

        public static void IsNull(object value, string comment)
        {
            if (null != value)
            {
                FireTestWarningEvent(new TestWarning(comment));
            }
        }

        public static void IsNotNull(object value, string expected, string actual)
        {
            IsNotNull(value, messageFormat, expected, actual);
        }

        public static void IsNotNull(object value, string format, params object[] args)
        {
            IsNotNull(value, string.Format(string.Format(format, args)));
        }

        public static void IsNotNull(object value, string comment)
        {
            if (null == value)
            {
                FireTestWarningEvent(new TestWarning(comment));
            }
        }

        public override string ToString()
        {
            return string.Format("\"{0}\" at\r\n    {1}",
                !string.IsNullOrEmpty(_comment) ? _comment : null,
                _source);
        }

        #endregion

        #region Class private methods

        private string getSource()
        {
            StackFrame frame = new StackFrame(3, true);

            Type type = frame.GetMethod().DeclaringType;

            string assembly = type.Assembly.ManifestModule.Name;
            string declaringType = type.Name;
            string method = frame.GetMethod().Name;
            string sourceFile = frame.GetFileName();
            int lineNumber = frame.GetFileLineNumber();

            string format = "Assembly:  {0}, Class: {1}, Method:  {2}\r\n    Source file:  {3} {4}";

            string source = string.Format(format,
                    assembly,
                    declaringType,
                    method,
                    sourceFile,
                    lineNumber != 0 ? " (" + lineNumber + ")" : null);
            return source;
        }

        #endregion
    }
}
