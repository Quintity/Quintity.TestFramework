using System;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    public class UnavailableReadsEventArgs
    {
        public List<TestSuite> TestSuites
        { get; set; }

        public UnavailableReadsEventArgs()
        {
            TestSuites = new List<TestSuite>();
        }
    }

    [DataContract]
    sealed public class TestSuite : TestScriptObjectContainer
    {
        public delegate bool TraverseTestTreeDelegate(TestScriptObject testScriptObject, object tag);

        public static void TraverseTestTree(TestScriptObject testScriptObject, TraverseTestTreeDelegate traverseTestTreeDelegate)
        {
            TraverseTestTree(testScriptObject, traverseTestTreeDelegate, null);
        }

        static bool m_continue;

        public static void TraverseTestTree(TestScriptObject testScriptObject, TraverseTestTreeDelegate traverseTestTreeDelegate, object tag)
        {
            m_continue = traverseTestTreeDelegate(testScriptObject, tag);

            if (m_continue)
            {
                if (testScriptObject is TestScriptObjectContainer)
                {
                    var container = testScriptObject as TestScriptObjectContainer;

                    foreach (TestScriptObject testscript in container.TestScriptObjects)
                    {
                        TraverseTestTree(testscript, traverseTestTreeDelegate, tag);

                        if (!m_continue)
                        {
                            break;
                        }
                    }
                }
            }
        }

        #region Class events

        public delegate void ExecutionBeginEventHandler(TestSuite testSuite, TestSuiteBeginExecutionArgs args);
        public static event ExecutionBeginEventHandler OnExecutionBegin;

        private void FireExecutionBeginEvent(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            OnExecutionBegin?.Invoke(testSuite, args);
        }

        public delegate void ExecutionCompleteEventHandler(TestSuite testSuite, TestSuiteResult testSuiteResult);
        public static event ExecutionCompleteEventHandler OnExecutionComplete;

        private void FireExecutionCompleteEvent(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            OnExecutionComplete?.Invoke(testSuite, testSuiteResult);
        }

        public delegate void TestPreprocessorBeginEventHandler(TestSuite testSuite, TestProcessorBeginExecutionArgs args);
        public static event TestPreprocessorBeginEventHandler OnTestPreprocessorBegin;

        private static void fireTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            OnTestPreprocessorBegin?.Invoke(testSuite, args);
        }

        public delegate void TestPreprocessorCompleteEventHandler(TestSuite testSuite, TestProcessorResult testProcessorResult);
        public static event TestPreprocessorCompleteEventHandler OnTestPreprocessorComplete;

        private static void fireTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            OnTestPreprocessorComplete?.Invoke(testSuite, testProcessorResult);
        }

        public delegate void TestPostprocessorBeginEventHandler(TestSuite testSuite, TestProcessorBeginExecutionArgs args);
        public static event TestPostprocessorBeginEventHandler OnTestPostprocessorBegin;

        private static void fireTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            OnTestPostprocessorBegin?.Invoke(testSuite, args);
        }

        public delegate void TestPostprocessorCompleteEventHandler(TestSuite testSuite, TestProcessorResult testProcessorResult);
        public static event TestPostprocessorCompleteEventHandler OnTestPostprocessorComplete;

        private static void fireTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            OnTestPostprocessorComplete?.Invoke(testSuite, testProcessorResult);
        }

        public delegate void UnavailableReadsEventHandler(TestSuite testSuite, UnavailableReadsEventArgs args);
        public static event UnavailableReadsEventHandler OnUnavailableReads;

        private static void fireUnavailableReads(TestSuite testSuite, UnavailableReadsEventArgs args)
        {
            OnUnavailableReads?.Invoke(testSuite, args);
        }

        #endregion

        #region Class data members

        private List<string> _allTags;
        private List<TestCase> _qualifiedTestCases = null;
        private int _available;

        [Browsable(false)]
        [IgnoreDataMember]
        public static TestSuite RootTestSuite
        { get; set; }

        [Browsable(false)]
        [IgnoreDataMember]
        public static UnavailableReadsEventArgs UnavailableReadsEventArgs
        { get; set; }

        [DataMember(Order = 29)]
        internal string _filePath;

        [IgnoreDataMember]
        [CategoryAttribute("Metadata"),
        DisplayName("File location"),
        DescriptionAttribute("Test suite file path."),
        ReadOnly(true),
        PropertyOrder(35)]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        [IgnoreDataMember]
        [CategoryAttribute("Metadata"),
        DisplayName("File name"),
        DescriptionAttribute("Test suite file name."),
        ReadOnly(true),
        PropertyOrder(34)]
        public string FileName => Path.GetFileName(_filePath);

        [DataMember(Order = 27)]
        private TestPreprocessor _testPreprocessor;

        [CategoryAttribute("Runtime Settings"),
        DisplayName("Test Pre-processor"),
        DescriptionAttribute("Test suite pre-processor definition"),
        PropertyOrder(40)]
        public TestPreprocessor TestPreprocessor
        {
            get { return _testPreprocessor; }
            set
            {
                if (_testPreprocessor != value)
                {
                    object oldValue = _testPreprocessor;
                    _testPreprocessor = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 28)]
        private TestPostprocessor _testPostProcessor;

        [CategoryAttribute("Runtime Settings"),
        DisplayName("Test Post-processor"),
        DescriptionAttribute("Test suite post-processor definition"),
        PropertyOrder(41)]
        public TestPostprocessor TestPostprocessor
        {
            get { return _testPostProcessor; }
            set
            {
                if (_testPostProcessor != value)
                {
                    object oldValue = _testPostProcessor;
                    _testPostProcessor = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [DataMember(Order = 26)]
        private string _project;

        [CategoryAttribute("General"),
        DescriptionAttribute("The application project"),
        PropertyOrder(30)]
        [Editor(typeof(TestTextBoxEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Project
        {
            get { return _project; }
            set
            {
                if (_project != value)
                {
                    object oldValue = _project;
                    _project = value;
                    notifyTestPropertyChangedEvent(new TestPropertyChangedEventArgs(oldValue, value));
                }
            }
        }

        [IgnoreDataMember]
        [Browsable(false)]
        new public TestScriptObjectCollection TestScriptObjects
        { get { return _testScriptObjects; } }

        #endregion

        #region Class constructors

        public TestSuite()
            : this(string.Empty, string.Empty, null)
        { }

        public TestSuite(string title, string filePath)
            : this(title, filePath, null)
        { }

        public TestSuite(string title, string filePath, TestSuite testSuite)
            : base(title, testSuite)
        {
            _filePath = filePath;
            _testPreprocessor = new TestPreprocessor(this);
            _testPostProcessor = new TestPostprocessor(this);
        }

        public TestSuite(TestSuite originalTestSuite, string filePath, TestSuite parent)
            : base(originalTestSuite, parent)
        {
            _filePath = filePath;
            _project = TestUtils.SafeCopyString(originalTestSuite._project);
            _testPreprocessor = new TestPreprocessor(originalTestSuite._testPreprocessor);
            _testPostProcessor = new TestPostprocessor(originalTestSuite.TestPostprocessor);
        }

        #endregion

        #region Class public methods

        public TestSuite AddTestSuite(string title, string filePath, int index = -1)
        {
            return AddTestSuite(new TestSuite(title, filePath), index);
        }

        public TestSuite AddTestSuite(TestSuite testSuite, int index = -1)
        {
            return AddTestScriptObject(testSuite, index) as TestSuite;
        }

        public TestCase AddTestCase(string title, int index = -1)
        {
            return AddTestCase(new TestCase(title), index);
        }

        public TestCase AddTestCase(TestCase testCase, int index = -1)
        {
            return AddTestScriptObject(testCase, index) as TestCase;
        }

        public void ClearTestScriptObjectCollection()
        {
            _testScriptObjects.Clear();
        }

        public int FindTestCaseIndex(TestCase testCase)
        {
            return FindTestScriptObjectIndex(testCase);
        }

        public int FindTestSuiteIndex(TestSuite testSuite)
        {
            return FindTestScriptObjectIndex(testSuite);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void SaveToFileX()
        {
            // Need to get actual filepath to write to OS
            string filePath = Core.TestProperties.ExpandString(_filePath);

            // Collapse internal suite file name to  
            _filePath = Core.TestProperties.FixupString(filePath, "TestSuites");

            fixupSuite();

            foreach (TestScriptObject testObject in TestScriptObjects)
            {
                if (testObject is TestSuite)
                {
                    // If child suite, write separate suite file out.
                    ((TestSuite)testObject).FileSave();
                }
            }

            SerializeToFile(this, filePath);

            expandSuite();
        }

        public void FileSave(string outputFile)
        {
            FilePath = outputFile;
            Write(this);
        }

        public void FileSave()
        {
            Write(this);
        }

        /// <summary>
        /// Makes a copy of the current test suite and writes to designated file path.
        /// This method assigns new system IDs to new current test suite (and processors).  
        /// Additionally does the same for each test case and the test cases test steps.
        /// Important:  For immediate child test suites, the parent suite ID is update only.  
        /// It does not iterate through any children or descendant test suites, updating test cases.
        /// These test suites will need to be handled individually and parent suites updated accordingly
        /// with newly saved child test suites.
        /// </summary>
        /// <param name="filePath"></param>
        public void FileSaveAs(string filePath)
        {
            _filePath = filePath;
            _systemId = Guid.NewGuid();
            _testPreprocessor.SetSystemID(Guid.NewGuid());
            _testPostProcessor.SetSystemID(Guid.NewGuid());

            foreach (var testScriptObject in _testScriptObjects)
            {
                if (testScriptObject is TestCase)
                {
                    var testCase = testScriptObject as TestCase;

                    // Is a test case, so update it's system ID and parent ID.
                    testCase.SetParent(this);
                    testCase.SetSystemID(Guid.NewGuid());

                    // Update each test case test step.
                    foreach (var testStep in testCase.TestSteps)
                    {
                        testStep.SetParent(testCase);
                        testStep.SetSystemID(Guid.NewGuid());
                    }
                }
                else
                {
                    testScriptObject.SetParent(this);
                }
            }

            Write(this);
        }

        /// <summary>
        /// Performs a backup to backup folder prior to saving.
        /// </summary>
        /// <param name="testSuite">Test suite to save.</param>
        /// <param name="createBackup">True for backup, false (default) not to backup.</param>
        public static void Write(TestSuite testSuite, bool createBackup = false)
        {
            XmlTextWriter xmlWriter = null;

            try
            {
                // First make backup if requested.
                if (createBackup)
                {
                    testSuite.createBackup();
                }

                // Write up queue information.
                xmlWriter = new XmlTextWriter($"{Core.TestProperties.ExpandString(testSuite.FilePath)}", null);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument();

                // Prepare suite results header.
                xmlWriter.WriteStartElement("TestSuite");

                // Write attributes.
                xmlWriter.WriteAttributeString("status", testSuite._status.ToString());

                // Suite header information.
                xmlWriter.WriteElementString("SystemID", testSuite._systemId.ToString());
                xmlWriter.WriteElementString("UserID", "", testSuite._userId);
                xmlWriter.WriteElementString("Title", "", testSuite._title);
                xmlWriter.WriteElementString("Author", "", testSuite._author);
                xmlWriter.WriteElementString("Created", "", testSuite._created.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                xmlWriter.WriteElementString("Project", "", testSuite._project);
                xmlWriter.WriteElementString("Reference", "", testSuite._reference);
                xmlWriter.WriteElementString("FunctionalArea", "", testSuite._functionalArea);

                // Write description
                xmlWriter.WriteStartElement("Description");
                xmlWriter.WriteCData(testSuite.Description);
                xmlWriter.WriteEndElement();

                // Write test propertes
                Core.TestProperties.Write(xmlWriter, testSuite.TestProperties);

                // Write pre and post processors to writer.
                testSuite.TestPreprocessor.Write(xmlWriter);
                testSuite.TestPostprocessor.Write(xmlWriter);

                // Write post-process method
                xmlWriter.WriteStartElement("PostProcess");
                //PostProcessor.Write(xmlWriter);
                xmlWriter.WriteEndElement();

                // Write elements
                xmlWriter.WriteStartElement("TestScriptObjects");

                // If test suites or cases exist, write....
                foreach (TestScriptObject testScriptObject in testSuite._testScriptObjects)
                {
                    if (testScriptObject is TestCase)
                    {
                        ((TestCase)testScriptObject).Write(xmlWriter);
                    }
                    else if (testScriptObject is TestSuite) // Must be test suite.
                    {
                        // Write test suite file information.
                        xmlWriter.WriteStartElement("TestScriptObject");
                        xmlWriter.WriteAttributeString("type", "TestSuite");

                        // If test suite is unavailble, keep entry but do not overwrite with placeholder test suite contents.
                        if (testScriptObject.Status != Core.Status.Unavailable)
                        {
                            Write(testScriptObject as TestSuite, createBackup);
                        }

                        xmlWriter.WriteString(((TestSuite)testScriptObject).FilePath);
                        xmlWriter.WriteEndElement();
                    }
                }

                // Clean up.
                xmlWriter.WriteEndElement();	// TestScriptObjects.

                xmlWriter.WriteElementString("FilePath", Core.TestProperties.FixupString(testSuite._filePath, "TestSuites", "TestHome"));

                xmlWriter.WriteEndElement();	// TestSuite header.	
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                if (xmlWriter != null)
                {
                    //Error opening results file, close (if open) and stop writting.
                    xmlWriter.Close();
                    xmlWriter = null;
                }
            }
        }

        public static TestSuite ReadFromFile(string filePath, TestSuite parentSuite = null)
        {
            TestSuite testSuite = null;

            if (parentSuite is null)
            {
                UnavailableReadsEventArgs = UnavailableReadsEventArgs ?? new UnavailableReadsEventArgs();
            }

            try
            {
                // Create new test suite
                testSuite = new TestSuite();
                RootTestSuite = RootTestSuite ?? testSuite;

                // Parent determined at run time (no longer static0
                testSuite._parentID = parentSuite is null ? Guid.Empty : parentSuite.SystemID;

                // Read document and prepare navigator.
                XPathDocument doc = new XPathDocument(Core.TestProperties.ExpandString(filePath));
                XPathNavigator nav = doc.CreateNavigator();

                // Move to element.
                nav.MoveToFirstChild();

                // Set fixed up filepath.
                testSuite._filePath = Core.TestProperties.FixupString(filePath, "TestSuites", "TestHome");

                // Get status attribute.
                Enum.TryParse(nav.GetAttribute("status", ""), out testSuite._status);

                // Extract object system ID.
                testSuite._systemId = new Guid(TestUtils.GetXPathValue(nav, "SystemID"));

                // Extract data fields.
                testSuite._userId = TestUtils.GetXPathValue(nav, "UserID");
                testSuite._title = TestUtils.GetXPathValue(nav, "Title");
                testSuite._author = TestUtils.GetXPathValue(nav, "Author");
                testSuite._created = Convert.ToDateTime(TestUtils.GetXPathValue(nav, "Created"));
                testSuite._project = TestUtils.GetXPathValue(nav, "Project");
                testSuite._reference = TestUtils.GetXPathValue(nav, "Reference");
                testSuite._description = TestUtils.GetXPathValue(nav, "Description");
                testSuite._functionalArea = TestUtils.GetXPathValue(nav, "FunctionalArea");
                testSuite._description = TestUtils.GetXPathValue(nav, "Description");

                XPathNodeIterator iterator = null;

                // Extract suite properties.
                //iterator = nav.Select("Properties");
                //iterator.MoveNext();
                //testSuite.ReadProperties(iterator.Current);

                // Extract pre-processor
                iterator = nav.Select("TestPreprocessor");
                iterator.MoveNext();
                testSuite._testPreprocessor = new TestPreprocessor(testSuite, iterator.Current);

                // Extract post processor.
                iterator = nav.Select("TestPostprocessor");
                iterator.MoveNext();
                testSuite._testPostProcessor = new TestPostprocessor(testSuite, iterator.Current);

                // Collect test case locations from group file.
                iterator = nav.Select("TestScriptObjects/TestScriptObject");

                while (iterator.MoveNext())
                {
                    TestScriptObject testObject = null;

                    // Get type of script object (i.e., TestSuite or TestCase).
                    var testtype = iterator.Current.GetAttribute("type", "");

                    if (testtype == "TestCase")
                    {
                        testObject = new TestCase(testSuite, iterator.Current);
                    }
                    else if (testtype == "TestSuite")
                    {
                        var suitePath = iterator.Current.Value;
                        testObject = ReadFromFile(iterator.Current.Value, testSuite);
                    }

                    testSuite.TestScriptObjects.Add(testObject);
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                testSuite._title = $"Test suite file \"{Path.GetFileName(filePath)}\" not found.";
                testSuite._description =
                    $"The designated file could not found.  " +
                    "If the file exists, verify the \"TestHome\" and \"TestSuite\" properties are properly configured." +
                    $"{Environment.NewLine}{Environment.NewLine}{e.ToString()}";
                testSuite._status = Core.Status.Unavailable;
                testSuite._filePath = filePath;
                UnavailableReadsEventArgs.TestSuites.Add(testSuite);
            }
            catch (System.Xml.XmlException e)
            {
                testSuite._title = $"Test suite file \"{Path.GetFileName(filePath)}\" could not be opened.";
                testSuite._description = "The designated  file does not appear to be a valid or well-formed." +
                     $"{Environment.NewLine}{Environment.NewLine}{e.ToString()}";
                testSuite._status = Core.Status.Unavailable;
                testSuite._filePath = filePath;
                UnavailableReadsEventArgs.TestSuites.Add(testSuite);
            }
            catch (Exception e)
            {
                testSuite._title = $"Error loading test suite \"{Path.GetFileName(filePath)}\".";
                testSuite._description = $"An unanticipated problem occurred opending test suite \"{Path.GetFileName(filePath)}." +
                     $"{Environment.NewLine}{Environment.NewLine}{e.ToString()}";
                testSuite._status = Core.Status.Unavailable;
                testSuite._filePath = filePath;
                UnavailableReadsEventArgs.TestSuites.Add(testSuite);
            }

            if (parentSuite is null)
            {
                fireUnavailableReads(RootTestSuite, UnavailableReadsEventArgs);
            }

            return testSuite;
        }

        public static TestSuite ReadFromOldFileFormat(string filePath, TestSuite parent = null)
        {
            TestSuite testSuite = DeserializeFromFile(filePath);
            //testSuite._filePath = filePath;

            //// If pre or post processor is defined, need to expand it its path.
            //if (null != testSuite.TestPreprocessor.TestAutomationDefinition.TestAssembly)
            //{
            //    Core.TestProperties.ExpandString(testSuite.TestPreprocessor.TestAutomationDefinition.TestAssembly);
            //}

            //if (null != testSuite.TestPostprocessor.TestAutomationDefinition.TestAssembly)
            //{
            //    Core.TestProperties.ExpandString(testSuite.TestPostprocessor.TestAutomationDefinition.TestAssembly);
            //}

            TestScriptObjectCollection testScriptObjects = new TestScriptObjectCollection();

            foreach (TestScriptObject testObject in testSuite.TestScriptObjects)
            {
                if (testObject is TestSuite)
                {
                    // If child is suite, need expand file path so can be loaded (i.e., deserialized)
                    TestSuite childSuite = testObject as TestSuite;
                    string expandedFilePath = Core.TestProperties.ExpandString(childSuite._filePath);
                    //childSuite = DeserializeFromFile(expandedFilePath);
                    childSuite = ReadFromOldFileFormat(expandedFilePath);

                    // Need to expand pre and post processor test assembly paths.
                    //if (null != childSuite.TestPreprocessor.TestAutomationDefinition.TestAssembly)
                    //{
                    //    Core.TestProperties.ExpandString(childSuite.TestPreprocessor.TestAutomationDefinition.TestAssembly);
                    //}

                    //if (null != childSuite.TestPostprocessor.TestAutomationDefinition.TestAssembly)
                    //{
                    //    Core.TestProperties.ExpandString(childSuite.TestPostprocessor.TestAutomationDefinition.TestAssembly);
                    //}

                    testScriptObjects.Add(childSuite);
                }
                else if (testObject is TestCase)
                {
                    TestCase testCase = testObject as TestCase;

                    foreach (TestStep testStep in testCase.TestSteps)
                    {
                        var definition = testStep.TestAutomationDefinition;

                        if (definition.TestAssembly != null)
                        {
                            // Expand test assembly path for execution.
                            definition.TestAssembly = Core.TestProperties.FixupString(definition.TestAssembly, "TestAssemblies");
                        }
                    }

                    testScriptObjects.Add(testObject);
                }
            }

            testSuite._testScriptObjects = testScriptObjects;

            return testSuite;
        }

        public static void SerializeToFile(TestSuite testSuite, string filePath)
        {
            firePrepareToSaveEvent(testSuite);
            TestArtifact.SerializeToFile(testSuite, filePath);
        }

        public static TestSuite DeserializeFromFile(string filePath)
        {
            return TestArtifact.DeserializeFromFile(typeof(TestSuite), filePath) as TestSuite;
        }

        public int AvailableTestCases()
        {
            _available = 0;

            TraverseTestTree(this, new TraverseTestTreeDelegate(availableTestCases));

            return _available;
        }

        public List<TestCase> Filter(TestFilter filter)
        {
            _qualifiedTestCases = new List<TestCase>();

            // Get qualified test cases based on tags.
            TraverseTestTree(this, new TraverseTestTreeDelegate(filterTestSuite), filter);

            // Triage test case listing based on sampling size.
            return getRandomSampleTestCases(filter);
        }

        #endregion

        #region Class private methods

        private void createBackup()
        {
            var sourceFile = Core.TestProperties.ExpandString(FilePath);
            var backupFolder = $"{Path.GetDirectoryName(sourceFile)}\\Backups";
            var backupFile = $"{backupFolder}\\{Path.GetFileName(sourceFile)}";

            // Determine  backups directory.
            //string backups = this.File.Substring(0, this.File.LastIndexOf('\\') + 1) + "Backups\\";

            // Determin backup file name.
            //string backup = backups + this.ShortFile.Replace(".ste", ".sbk");

            // If backup directory doesn't exist, make it.
            if (!System.IO.Directory.Exists(backupFolder))
            {
                System.IO.Directory.CreateDirectory(backupFolder);
            }

            // Delete backup file if exists (removes readonly attribute if set).
            if (System.IO.File.Exists(backupFile))
            {
                System.IO.File.SetAttributes(backupFile, System.IO.FileAttributes.Normal);
                System.IO.File.Delete(backupFile);
            }

            // Copy file if exists....
            if (System.IO.File.Exists(sourceFile))
            {
                System.IO.File.Copy(sourceFile, backupFile);
            }
        }

        [Obsolete]
        private void fixupSuite()
        {
            _filePath = Core.TestProperties.FixupString(this._filePath, "TestSuites");
            _filePath = Core.TestProperties.FixupString(this._filePath, "TestHome");

            // If pre or post processor is defined, need to fix up its path.
            if (null != TestPreprocessor.TestAutomationDefinition.TestAssembly)
            {
                Core.TestProperties.FixupString(TestPreprocessor.TestAutomationDefinition.TestAssembly, "TestAssemblies");
                Core.TestProperties.FixupString(TestPreprocessor.TestAutomationDefinition.TestAssembly, "TestHome");
            }

            if (null != TestPostprocessor.TestAutomationDefinition.TestAssembly)
            {
                Core.TestProperties.FixupString(TestPostprocessor.TestAutomationDefinition.TestAssembly, "TestAssemblies");
                Core.TestProperties.FixupString(TestPostprocessor.TestAutomationDefinition.TestAssembly, "TestHome");
            }

            foreach (TestScriptObject testObject in TestScriptObjects)
            {
                // Iterate through children
                if (testObject is TestSuite)
                {
                    // If child suite, write separate suite file out.
                    ((TestSuite)testObject).fixupSuite();
                }
                else if (testObject is TestCase)
                {
                    TestCase testCase = testObject as TestCase;

                    foreach (TestStep testStep in testCase.TestSteps)
                    {
                        var definition = testStep.TestAutomationDefinition;

                        if (definition.TestAssembly != null)
                        {
                            // Collapse test assembly path before writing (loading will expand).
                            definition.TestAssembly = Core.TestProperties.FixupString(definition.TestAssembly, "TestAssemblies");
                            definition.TestAssembly = Core.TestProperties.FixupString(definition.TestAssembly, "TestHome");
                        }
                    }
                }
            }
        }

        [Obsolete]
        // TODO - remove.
        private void expandSuite()
        {
            // If pre or post processor is defined, need to fix up its path.
            if (null != TestPreprocessor.TestAutomationDefinition.TestAssembly)
            {
                Core.TestProperties.ExpandString(TestPreprocessor.TestAutomationDefinition.TestAssembly);
            }

            if (null != TestPostprocessor.TestAutomationDefinition.TestAssembly)
            {
                Core.TestProperties.ExpandString(TestPostprocessor.TestAutomationDefinition.TestAssembly);
            }

            foreach (TestScriptObject testObject in TestScriptObjects)
            {
                // Iterate through children
                if (testObject is TestSuite)
                {
                    // If child suite, write separate suite file out.
                    ((TestSuite)testObject).expandSuite();
                }
                else if (testObject is TestCase)
                    if (testObject is TestCase)
                    {
                        TestCase testCase = testObject as TestCase;

                        foreach (TestStep testStep in testCase.TestSteps)
                        {
                            var definition = testStep.TestAutomationDefinition;

                            if (definition.TestAssembly != null)
                            {
                                // Collapse test assembly path before writing (loading will expand).
                                definition.TestAssembly = Core.TestProperties.ExpandString(definition.TestAssembly);
                                definition.TestAssembly = Core.TestProperties.ExpandString(definition.TestAssembly);
                            }
                        }
                    }
            }
        }


        private bool availableTestCases(TestScriptObject testScriptObject, object tag)
        {
            if (testScriptObject is TestCase && testScriptObject.Status == Core.Status.Active)
            {
                if (isTestCaseReachable((TestCase)testScriptObject))
                {
                    _available = _available + 1;
                }
            }

            return true;
        }


        private List<TestCase> getRandomSampleTestCases(TestFilter filter)
        {
            List<TestCase> sampledTestCases = new List<TestCase>();

            if (filter != null && filter.SamplingRate != 100)
            {
                // Based on size of lot, determine sample size (in test case indicies).
                int samplesize = (int)Math.Round((decimal)((_qualifiedTestCases.Count * filter.SamplingRate) / 100), MidpointRounding.AwayFromZero);

                // If sample size 0, then force at least one test case for execution.
                samplesize = samplesize == 0 ? 1 : samplesize;

                int randInt;
                int count = 1;
                Random random = new Random();

                // Iterate until have sample size of unique test case indices.
                while (true)
                {
                    randInt = random.Next(0, _qualifiedTestCases.Count);

                    sampledTestCases.Add(_qualifiedTestCases[randInt]);
                    _qualifiedTestCases.RemoveAt(randInt);

                    if (count == samplesize)
                    {
                        break;
                    }

                    count++;
                }
            }
            else
            {
                sampledTestCases.AddRange(_qualifiedTestCases);
            }

            return sampledTestCases;
        }

        private bool filterTestSuite(TestScriptObject testScriptObject, object tag)
        {
            if (testScriptObject is TestCase && isQualified(testScriptObject as TestCase, tag as TestFilter))
            {
                _qualifiedTestCases.Add((TestCase)testScriptObject);
            }

            return true;
        }

        private bool isQualified(TestCase testCase, TestFilter filter)
        {
            bool qualified = false;

            // If there are filter tags, process, otherwise call it good.
            if (filter != null && filter.Tags.Count != 0)
            {
                if (!String.IsNullOrEmpty(testCase.Tags))
                {
                    List<string> caseTags = TestUtils.ToList(testCase.Tags);

                    foreach (string filterTag in filter.Tags)
                    {
                        foreach (string caseTag in caseTags)
                        {
                            if (caseTag.Equals(filterTag))
                            {
                                qualified = true;
                                break;
                            }
                        }

                        if (qualified)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    foreach (string filterTag in filter.Tags)
                    {
                        if (filterTag == null)
                        {
                            qualified = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                qualified = true;
            }

            return qualified;
        }

        private bool isTestCaseReachable(TestCase testCase)
        {
            bool isReachable = true;

            TestSuite parentSuite = this.Find(testCase.ParentID) as TestSuite;

            while (true)
            {
                if (parentSuite.Status != Core.Status.Active)
                {
                    isReachable = false;
                    break;
                }

                if (parentSuite.ParentID == Guid.Empty)
                {
                    break;
                }

                parentSuite = this.Find(parentSuite.ParentID) as TestSuite;

                if (null == parentSuite)
                {
                    break;
                }
            }

            return isReachable;
        }

        //TODO - Remove if works in TestTreeView
        //public List<string> GatherAllTags()
        //{
        //    _allTags = new List<string>();

        //    TestExecutor.TraverseTestTree(this, new TestExecutor.TraverseTestTreeDelegate(gatherAllTags));

        //    return _allTags.Distinct<string>().ToList<string>();
        //}



        //private bool gatherAllTags(TestScriptObject testScriptObject, object tag)
        //{
        //    if (testScriptObject is TestCase)
        //    {
        //        string tags = ((TestCase)(testScriptObject)).Tags;

        //        if (null != tags)
        //        {
        //            _allTags.AddRange(TestUtils.ToList(tags));
        //        }
        //    }

        //    return true;
        //}

        private bool isQualifiedTestCase(TestCase testCase)
        {
            return _qualifiedTestCases == null ||
                _qualifiedTestCases.FindIndex(x => x.SystemID.Equals(testCase.SystemID)) != -1 ? true : false;
        }

        #endregion

        #region Class internal methods

        public TestSuiteResult Execute(List<TestCase> discreteTestCases)
        {
            string virtualUser = Thread.CurrentThread.Name;

            FireExecutionBeginEvent(this, new TestSuiteBeginExecutionArgs(virtualUser));

            _qualifiedTestCases = discreteTestCases;

            TestSuiteResult testSuiteResult = new TestSuiteResult();
            testSuiteResult.SetReferenceID(SystemID);
            testSuiteResult.SetVirtualUser(virtualUser);

            // Save copy of test property collection for restoration later (maintains scope)
            TestPropertyCollection savedTestPropertyCollection = new TestPropertyCollection(Core.TestProperties.TestPropertyCollection);

            AddRuntimeTestPropertiesToTestProperties();

            List<TestScriptObject> activeTestScriptObjects = getActiveChildren();

            testSuiteResult.SetStartTime(DateTime.Now);

            if (activeTestScriptObjects.Count > 0)
            {
                // Execute pre-processor
                TestProcessorResult processorResult = new TestProcessorResult();

                if (TestPreprocessor.Status == Core.Status.Active)
                {
                    fireTestPreprocessorBegin(this, new TestProcessorBeginExecutionArgs(virtualUser));

                    processorResult = TestPreprocessor.Execute();

                    fireTestPreprocessorComplete(this, processorResult);

                    //Update test suite result.
                    testSuiteResult.SetTestPreprocessorResult(processorResult);
                }

                if ((processorResult.TestVerdict != TestVerdict.Fail && processorResult.TestVerdict != TestVerdict.Error) ||
                    TestPreprocessor.OnFailure != OnFailure.Stop)
                {
                    // Iterate through suites test script objects
                    foreach (TestScriptObject testScriptObject in TestScriptObjects)
                    {
                        // Only execute Active objects.
                        if (testScriptObject.Status == Core.Status.Active)
                        {
                            if (testScriptObject is TestCase)
                            {
                                TestCase testCase = testScriptObject as TestCase;

                                if (isQualifiedTestCase(testCase))
                                {
                                    var testScriptResult = testCase.Execute();
                                    testSuiteResult.IncrementCounter(testScriptResult.TestVerdict);
                                    testSuiteResult.AddResult(testScriptResult);
                                }
                            }
                            else if (testScriptObject is TestSuite)
                            {
                                TestSuite childSuite = testScriptObject as TestSuite;
                                var testScriptResult = childSuite.Execute(discreteTestCases);
                                testSuiteResult.IncrementCounters(testScriptResult);
                                testSuiteResult.AddResult(testScriptResult);
                            }
                        }
                    }
                }
                else
                {
                    if (!TestPreprocessor.IgnoreResult)
                    {
                        testSuiteResult.SetTestVerdict(processorResult.TestVerdict);
                        testSuiteResult.SetTestMessage("The test pre-processor failed, test suite execution stopped per setting.");
                    }
                    else
                    {
                        testSuiteResult.SetTestMessage("The test pre-processor failure ignored per setting.");
                    }
                }

                // Execute post-processor
                if (TestPostprocessor.Status == Core.Status.Active)
                {
                    fireTestPostprocessorBegin(this, new TestProcessorBeginExecutionArgs(virtualUser));

                    processorResult = TestPostprocessor.Execute();

                    fireTestPostprocessorComplete(this, processorResult);

                    if (!TestPostprocessor.IgnoreResult)
                    {
                        //Update test suite result.
                        testSuiteResult.SetTestPostprocessorResult(processorResult);
                    }
                }
            }
            else
            {
               // testSuiteResult.SetTestVerdict(TestVerdict.DidNotExecute);
                testSuiteResult.SetTestMessage("The test suite is set to \"Active\", however it does not contain any active test cases or suites.");
            }

            testSuiteResult.SetEndTime(DateTime.Now);

            testSuiteResult.FinalizeVerdict();

            // Restore preserved test property collection.
            Core.TestProperties.SetTestPropertyCollection(savedTestPropertyCollection);

            FireExecutionCompleteEvent(this, testSuiteResult);

            return testSuiteResult;
        }

        #endregion
    }
}
