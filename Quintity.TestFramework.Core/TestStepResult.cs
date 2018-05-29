using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestStepResult : TestScriptResult
    {
        #region Class data members

        [DataMember(Order = 10)]
        protected TestCheckCollection _testChecks;

        [DataMember(Order = 11)]
        protected TestWarningCollection _testWarnings;

        [DataMember(Order = 12)]
        protected TestDataCollection _testData;

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public TestCheckCollection TestChecks
        { get { return _testChecks; } }

        [IgnoreDataMember]
        public TestWarningCollection TestWarnings
        { get { return _testWarnings; } }

        [IgnoreDataMember]
        public TestDataCollection TestData
        { get { return _testData; } }

        #endregion

        #region Class constructors

        public TestStepResult(TestVerdict testVerdict, string testMesssage)
            : base()
        {
            _testVerdict = testVerdict;
            _testMessage = testMesssage;
        }

        public TestStepResult(TestVerdict testVerdict)
            : this(testVerdict, null)
        { }

        public TestStepResult()
        { }

        #endregion

        #region Class public methods

        public static void SerializeToFile(TestStepResult testStepResult, string filePath)
        {
            TestArtifact.SerializeToFile(testStepResult, filePath);
        }

        public static TestStepResult DeserializeFromFile(string filePath)
        {
            return TestArtifact.DeserializeFromFile(typeof(TestStepResult), filePath) as TestStepResult;
        }

        public override string ToString()
        {
            return string.Format("{0}\r\n\r\n{1}\r\n\r\n{2}\r\n\r\n{3}",
                base.ToString(),
                _testWarnings != null ? _testWarnings.ToString() : null,
                _testChecks != null ? _testChecks.ToString() : null,
                _testData != null ? _testData.ToString() : null);
        }

        #endregion

        #region Class internal methods

        internal void SetTestChecks(TestCheckCollection testChecks)
        {
            _testChecks = new TestCheckCollection(testChecks);
        }

        internal void SetTestWarnings(TestWarningCollection testWarnings)
        {
            _testWarnings = new TestWarningCollection(testWarnings);
        }

        internal void SetTestData(TestDataCollection testData)
        {
            _testData = new TestDataCollection(testData);
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
