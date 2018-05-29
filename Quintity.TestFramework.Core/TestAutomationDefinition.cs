using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    // TODO this could be a problem 05/01/2018
    //[Editor(typeof(TestAutomationDefinitionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public class TestAutomationDefinition : TestObject
    {
        #region Class data members

        [DataMember(Order = 10)]
        private string _testAssembly;

        public string TestAssembly
        {
            get { return _testAssembly; }
            set { _testAssembly = value; }
        }

        [DataMember(Order = 11)]
        private string _testClass;

        public string TestClass
        {
            get { return _testClass; }
            set { _testClass = value; }
        }

        [DataMember(Order = 12)]
        private string _testMethod;

        public string TestMethod
        {
            get { return _testMethod; }
            set { _testMethod = value; }
        }

        [DataMember(Order = 13)]
        private TestParameterCollection _testParameters;

        public TestParameterCollection TestParameters
        {
            get { return _testParameters; }
            set { _testParameters = value; }
        }

        #endregion

        public bool IsCompleted()
        {
            return (TestAssembly != null && TestClass != null && TestMethod != null && TestParameters != null) ? true : false;
        }

        #region Class constructors

        public TestAutomationDefinition()
            : base()
        {
            _testParameters = new TestParameterCollection();
        }

        public TestAutomationDefinition(TestAutomationDefinition originalTestAutomationDefinition)
            : this()
        {
            this._testAssembly = TestUtils.SafeCopyString(originalTestAutomationDefinition._testAssembly);
            this._testClass = TestUtils.SafeCopyString(originalTestAutomationDefinition._testClass);
            this._testMethod = TestUtils.SafeCopyString(originalTestAutomationDefinition._testMethod);

            foreach (TestParameter parameter in originalTestAutomationDefinition._testParameters)
            {
                _testParameters.Add(new TestParameter(parameter));
            }
        }

        public TestAutomationDefinition(string testAssembly, string testClass, string testMethod, TestParameterCollection testParameters)
            : this()
        {
            this._testAssembly = testAssembly;
            this._testClass = testClass;
            this._testMethod = testMethod;
            this._testParameters = testParameters;
        }

        public TestAutomationDefinition(XPathNavigator navigator)
        {
            var testAssembly = TestUtils.GetXPathValue(navigator, "TestAssembly");
            _testAssembly = TestProperties.FixupString(testAssembly, "TestAssemblies", "TestHome");
            _testClass = TestUtils.GetXPathValue(navigator, "TestClass");
            _testMethod = TestUtils.GetXPathValue(navigator, "TestMethod");
            _testParameters = TestParameterCollection.ReadParameters(navigator);
        }

        #endregion

        #region Class internal methods

        internal class ResultStruct
        {
            public DateTime StartTime
            { get; set; }

            public DateTime EndTime
            { get; set; }

            public TestVerdict TestVerdict
            { get; set; }

            public string TestMessage
            { get; set; }

            public TestCheckCollection TestChecks
            { get; set; }

            public TestWarningCollection TestWarnings
            { get; set; }

            public TestDataCollection TestData
            { get; set; }

            public ResultStruct()
            {
                TestChecks = new TestCheckCollection();
                TestWarnings = new TestWarningCollection();
                TestData = new TestDataCollection();
            }
        }

        internal ResultStruct Invoke(Dictionary<int, TestClassBase> testClassDictionary)
        {
            ResultStruct resultStruct = new ResultStruct();

            try
            {
                Assembly assembly = TestReflection.LoadTestAssembly(TestProperties.ExpandString(TestAssembly));
                Type testClassType = TestReflection.GetTestClass(assembly, TestClass);

                TestAssert.IsNotNull(testClassType, "The test class specified \"{0}\" cannot be located or is not a valid test class.  " +
                    "Check the test class's TestClassAttribute has been set.", TestClass);

                Type[] types = TestParameters.GetParameterTypes();

                MethodInfo testMethod = TestReflection.GetTestMethod(testClassType, TestMethod, types);

                TestAssert.IsNotNull(testMethod, "The test method specified \"{0}\" cannot be located or it is not a valid test method.  " +
                    "Check the test method signature is correct and the TestMethodAttribute had been set.", TestMethod);

                TestClassBase testClassInstance;
                var virtualUser = Thread.CurrentThread.Name;
                int hashcode = testClassType.GetHashCode() + virtualUser.GetHashCode();

                if (testClassDictionary != null && testClassDictionary.TryGetValue(hashcode, out testClassInstance))
                {
                    testClassInstance.TestMessage = null;
                    testClassInstance.ResetTestCheckCollection();
                    testClassInstance.ResetTestWarningCollection();
                    testClassInstance.ResetTestDataCollection();
                    testClassInstance.TestVerdict = TestVerdict.Pass;
                }
                else
                {
                    testClassInstance = Activator.CreateInstance(testClassType) as TestClassBase;
                    testClassInstance.VirtualUser = virtualUser;

                    if (testClassDictionary != null)
                    {
                        testClassDictionary.Add(hashcode, testClassInstance);
                    }
                }

                object[] values = TestParameters.GetParameterValues();

                PropertyInfo property = null;

                //TODO - this needs to be abstractedfrom TestExecutor class.
                //if (!TestExecutor.SuppressExecution)
                if (true)
                {
                    resultStruct.StartTime = DateTime.Now;

                    resultStruct.TestVerdict = (TestVerdict)testMethod.Invoke(testClassInstance, values);

                    resultStruct.EndTime = DateTime.Now;

                    property = testClassType.GetProperty("TestMessage");
                    resultStruct.TestMessage = (string)property.GetValue(testClassInstance, null);
                }
                else
                {
                    resultStruct.TestVerdict = TestVerdict.Pass;
                    resultStruct.TestMessage = "Test runtime execution has been suppressed.";
                }

                // Get properties
                property = testClassType.GetProperty("TestChecks");
                resultStruct.TestChecks = (TestCheckCollection)property.GetValue(testClassInstance, null);

                property = testClassType.GetProperty("TestWarnings");
                resultStruct.TestWarnings = (TestWarningCollection)property.GetValue(testClassInstance, null);

                property = testClassType.GetProperty("TestData");
                resultStruct.TestData = (TestDataCollection)property.GetValue(testClassInstance, null);

                resultStruct = determineTestVerdict(resultStruct);
            }
            catch (Exception e)
            {
                if (e is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = e as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                }

                resultStruct.TestVerdict = TestVerdict.Error;
                resultStruct.TestMessage += e.ToString();
            }

            return resultStruct;
        }

        #endregion

        #region Class public methods

        public override bool Equals(object obj)
        {
            bool equal = false;

            if (obj is TestAutomationDefinition)
            {
                var target = obj as TestAutomationDefinition;

                if (isEqual(_testAssembly, target._testAssembly) &&
                    isEqual(_testClass, target._testClass) &&
                    isEqual(_testMethod, target._testMethod) &&
                    areParamsEqual(_testParameters, target._testParameters))
                {
                    equal = true;
                }
            }

            return equal;
        }

        private bool areParamsEqual(TestParameterCollection testParameters1, TestParameterCollection testParameters2)
        {
            bool equal = true;

            if (testParameters1.Count == testParameters2.Count)
            {
                for (int i = 0; i < testParameters1.Count; i++)
                {
                    if (!testParameters1[i].Equals(testParameters2[i]))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            else
            {
                equal = false;
            }


            return equal;
        }

        private bool isEqual(string string1, string string2)
        {
            bool equal = true;

            // Check to see if one is null and the other not.
            equal = ((string1 == null && string2 != null) || (string1 != null && string2 == null)) ? false : true;

            // So far, assume still equal.  Both are null or have string content.  Grab on and test for nullness...
            if (equal)
            {
                if (string1 != null)
                {
                    // Make upper chase and compare.
                    equal = string1.ToUpper().Equals(string2.ToUpper());
                }
            }

            return equal;
        }

        private bool areParamsEqual(TestAutomationDefinition target)
        {
            bool isEqual = true;

            if (this._testParameters.Count == target._testParameters.Count)
            {
                for (int i = 0; i < this._testParameters.Count; i++)
                {
                    if (!this._testParameters[i].DisplayName.ToUpper().Equals(target._testParameters[i].DisplayName.ToUpper()) ||
                        !this._testParameters[i].TypeAsString.ToUpper().Equals(target._testParameters[i].TypeAsString.ToUpper()) ||
                        !this._testParameters[i].ValueAsString.ToUpper().Equals(target._testParameters[i].ValueAsString.ToUpper()))
                    {
                        isEqual = false;
                    }
                }
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Automation definition:\r\nTest assembly:  {0}\r\nTest class:  {1}\r\nTestMethod:  {2}\r\n{3}",
                TestAssembly != null ? TestAssembly : "Undefined",
                TestClass != null ? TestClass : "Undefined",
                TestMethod != null ? TestMethod : "Undefined",
                TestParameters.ToString());
        }

        internal void Write(XmlTextWriter xmlWriter)
        {
            // Write test method.
            xmlWriter.WriteStartElement("TestAutomationDefinition");

            // Automation values.
            xmlWriter.WriteElementString("TestAssembly", TestProperties.FixupString(_testAssembly, "TestAssemblies", "TestHome"));
            xmlWriter.WriteElementString("TestClass", _testClass);
            xmlWriter.WriteElementString("TestMethod", _testMethod);

            xmlWriter.WriteStartElement("TestParameters");

            // If test parameters exist, write.
            if (TestParameters != null)
            {
                foreach (TestParameter testParameter in _testParameters)
                {
                    // Writes parent element name.
                    xmlWriter.WriteStartElement("TestParameter");

                    // Write element attributes for name.
                    xmlWriter.WriteElementString("DisplayName", "", testParameter.DisplayName);
                    xmlWriter.WriteElementString("Name", testParameter.Name);
                    xmlWriter.WriteElementString("TypeAsString", testParameter.TypeAsString);
                    xmlWriter.WriteElementString("ValueAsString", testParameter.ValueAsString);

                    // Ends parent element.
                    xmlWriter.WriteEndElement();
                }
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
        }

        #endregion

        #region Class private methods

        ResultStruct determineTestVerdict(ResultStruct resultStruct)
        {
            if (resultStruct.TestVerdict == TestVerdict.Pass)
            {
                if (resultStruct.TestChecks.FindAll(x => x.TestVerdict == TestVerdict.Fail || x.TestVerdict == TestVerdict.Error).Count > 0)
                {
                    resultStruct.TestVerdict = TestVerdict.Fail;
                }
            }

            return resultStruct;
        }

        private List<object> getRuntimeParameters()
        {
            List<object> runtimeParams = new List<object>();

            foreach (TestParameter testParameter in TestParameters)
            {
                if (testParameter.ValueAsString is string)
                {
                    testParameter.ValueAsString = TestProperties.ExpandString((string)testParameter.ValueAsString);
                }

                Type type = Type.GetType(testParameter.TypeAsString);

                object value;

                TestUtils.FromString(testParameter.ValueAsString, type, out value);
            }

            return runtimeParams;
        }

        #endregion
    }
}
