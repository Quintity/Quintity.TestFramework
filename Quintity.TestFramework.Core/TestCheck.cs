/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// The TestCheck object records the results of a inprocess test validation check.
    /// </summary>
    /// 
    [DataContract]
    public class TestCheck : TestArtifact
    {
        #region Event and delegate definitions

        public delegate void TestCheckEventHandler(TestCheck testCheck);
        public static event TestCheckEventHandler OnTestCheck;

        internal static void FireTestCheckEvent(TestCheck testCheck)
        {
            if (TestCheck.OnTestCheck != null)
            {
                TestCheck.OnTestCheck(testCheck);
            }
        }

        #endregion

        #region Data members
        private static readonly string messageFormat = "Expected:  {0}, Actual:  {1}";

        [DataMember(Order = 0)]
        private string _virtualUser;    // TestCheck description.

        [DataMember(Order = 1)]
        private string _description;    // TestCheck description.

        [DataMember(Order = 2)]
        private TestVerdict _verdict;  	// Boolean checkpoint.

        [DataMember(Order = 3)]
        private string _comment;		// Optional comment.

        [DataMember(Order = 4)]
        private string _source;

        #endregion

        #region Class constructors

        /// <summary>
        /// Constructor with name, verdict and comments for a runtime test check validation.
        /// </summary>
        /// <param name="description">Name of test check.</param>
        /// <param name="verdict">Verdict of test check.</param>
        /// <param name="comment">Optional comment of the test check.</param>
        internal TestCheck(string description, TestVerdict verdict, string comment)
        {
            _virtualUser = Thread.CurrentThread.Name;
            _description = description;
            _verdict = verdict;
            _comment = comment;
            _source = getSource();
        }

        #endregion

        #region Class properties

        /// <summary>
        /// Returns the virtual user of the test check.
        /// </summary>
        [IgnoreDataMember]
        public string VirtualUser
        {
            get { return _virtualUser; }
        }

        /// <summary>
        /// Returns the test verdict of a test check;
        /// </summary>
        [IgnoreDataMember]
        public TestVerdict TestVerdict
        {
            get { return _verdict; }
        }

        /// <summary>
        /// Returns the name of the test check.
        /// </summary>
        [IgnoreDataMember]
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Returns the optional comment associated with test check.
        /// </summary>
        [IgnoreDataMember]
        public string Comment
        {
            get { return _comment; }
        }

        [IgnoreDataMember]
        public string Source
        { get { return _source; } }

        #endregion properties

        #region Class public methods

        public static TestCheck Fail(string description, bool throwOnFailedAssertion = false)
        {
            return Fail(description, null, throwOnFailedAssertion);
        }

        public static TestCheck Fail(string description, string comment, bool throwOnFailedAssertion = false)
        {
            var testCheck = new TestCheck(description, TestVerdict.Fail, comment);

            FireTestCheckEvent(testCheck);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck Fail(string description, string expected, string actual, bool throwOnFailedAssertion = false)
        {
            return Fail(description, messageFormat, expected, actual, throwOnFailedAssertion);
        }

        public static TestCheck Fail(string description, string format, params object[] args)
        {
            var testCheck = new TestCheck(description, TestVerdict.Fail, string.Format(format, args));

            FireTestCheckEvent(testCheck);

            return testCheck;
        }

        public static TestCheck Pass(string description, bool throwOnFailedAssertion = false)
        {
            return Pass(description, null, throwOnFailedAssertion);
        }

        public static TestCheck Pass(string description, string comment, bool throwOnFailedAssertion = false)
        {
            var testCheck = new TestCheck(description, TestVerdict.Pass, comment);

            FireTestCheckEvent(testCheck);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck Pass(string description, string expected, string actual, bool throwOnFailedAssertion = false)
        {
            var testCheck = Pass(description, messageFormat, expected, actual);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck Pass(string description, string format, params object[] args)
        {
            var testCheck = new TestCheck(description, TestVerdict.Pass, string.Format(format, args));

            FireTestCheckEvent(testCheck);

            return testCheck;
        }

        public static TestCheck IsFalse(string description, bool condition, bool throwOnFailedAssertion = false)
        {
            return IsFalse(description, condition, null, throwOnFailedAssertion);
        }

        public static TestCheck IsFalse(string description, bool condition, string comment, bool throwOnFailedAssertion = false)
        {
            var testCheck = new TestCheck(description, condition ? TestVerdict.Fail : TestVerdict.Pass, comment);

            FireTestCheckEvent(testCheck);
            
            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsFalse(string description, bool condition, string expected, string actual, bool throwOnFailedAssertion = false)
        {
            var testCheck = IsFalse(description, condition, messageFormat, expected, actual);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsFalse(string description, bool condition, string format, params object[] args)
        {
            var testCheck = IsFalse(description, condition, string.Format(format, args));

            FireTestCheckEvent(testCheck);

            return testCheck;
        }

        public static TestCheck IsTrue(string description, bool condition, bool throwOnFailedAssertion = false)
        {
            return IsTrue(description, condition, null, throwOnFailedAssertion);
        }

        public static TestCheck IsTrue(string description, bool condition, string comment, bool throwOnFailedAssertion = false)
        {
            var testCheck = new TestCheck(description, condition ? TestVerdict.Pass : TestVerdict.Fail, comment);

            FireTestCheckEvent(testCheck);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsTrue(string description, bool condition, string expected, string actual, bool throwOnFailedAssertion = false)
        {
            var testCheck = IsTrue(description, condition, messageFormat, expected, actual);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsTrue(string description, bool condition, string format, params object[] args)
        {
            var testCheck = new TestCheck(description, condition ? TestVerdict.Pass : TestVerdict.Fail, string.Format(format, args));

            FireTestCheckEvent(testCheck);

            return testCheck;
        }

        public static TestCheck IsNull(string description, object value, bool throwOnFailedAssertion = false)
        {
            return IsNull(description, value, null, throwOnFailedAssertion);
        }

        public static TestCheck IsNull(string description, object value, string comment, bool throwOnFailedAssertion = false)
        {
            var testCheck = new TestCheck(description, null == value ? TestVerdict.Pass : TestVerdict.Fail, comment);

            FireTestCheckEvent(testCheck);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsNull(string description, object value, string expected, string actual, bool throwOnFailedAssertion = false)
        {
            var testCheck = IsNull(description, value, messageFormat, expected, actual);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsNull(string description, object value, string format, params object[] args)
        {
            var testCheck = new TestCheck(description, null == value ? TestVerdict.Pass : TestVerdict.Fail, string.Format(format, args));

            FireTestCheckEvent(testCheck);

            return testCheck;
        }

        public static TestCheck IsNotNull(string description, object value, bool throwOnFailedAssertion = false)
        {
            return IsNotNull(description, value, null, throwOnFailedAssertion);
        }

        public static TestCheck IsNotNull(string description, object value, string comment, bool throwOnFailedAssertion = false)
        {
            var testCheck = new TestCheck(description, null != value ? TestVerdict.Pass : TestVerdict.Fail, comment);

            FireTestCheckEvent(testCheck);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsNotNull(string description, object value, string expected, string actual, bool throwOnFailedAssertion = false)
        {
            var testCheck = IsNotNull(description, value, messageFormat, expected, actual);

            if (testCheck._verdict == TestVerdict.Fail && throwOnFailedAssertion)
            {
                throwTestCheckFailedException(testCheck);
            }

            return testCheck;
        }

        public static TestCheck IsNotNull(string description, object value, string format, params object[] args)
        {
            var testCheck = new TestCheck(description, null != value ? TestVerdict.Pass : TestVerdict.Fail, string.Format(format, args));

            FireTestCheckEvent(testCheck);

            return testCheck;
        }

        public override string ToString()
        {
            return string.Format("\"{0}\"\r\n    Test verdict:  {1}\r\n{2}    {3}",
                _description,
                _verdict,
                !string.IsNullOrEmpty(_comment) ? "    Comment:  " + _comment + "\r\n" : null,
                Source);
        }

        #endregion

        #region Class private methods

        private static void throwTestCheckFailedException(TestCheck testCheck)
        {
            throw new TestCheckFailedException(testCheck);
        }

        private string getSource()
        {
            StackFrame frame = new StackFrame(4, true);

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
