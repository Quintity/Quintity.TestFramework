using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Runtime
{
    [CollectionDataContract
      (Name = "TestListeners",
      ItemName = "TestListener")]
    public class TestListenerCollection : List<TestListenerDescriptor>
    {
        #region Class data members

        #endregion

        #region Constructor

        public TestListenerCollection()
        { }

        public TestListenerCollection(TestListenerDescriptor[] testListeners) : base(testListeners)
        { }

        public TestListenerCollection(List<TestListenerDescriptor> testListeners) : base(testListeners)
        { }

        #endregion

        #region Class public methods

        public static void SerializeToFile(TestListenerCollection testListeners, string filePath)
        {
            XmlWriter writer = null;

            try
            {
                // Fixup assembly path to TestListeners definition.
                foreach (TestListenerDescriptor descriptor in testListeners)
                {
                    if (!string.IsNullOrEmpty(descriptor.Assembly))
                    {
                        TestProperties.FixupString(descriptor.Assembly, "TestConfigs");
                    }
                }

                DataContractSerializer serializer = new DataContractSerializer(typeof(TestListenerCollection));

                var settings = new XmlWriterSettings() { Indent = true };

                using (writer = XmlWriter.Create(filePath, settings))
                {
                    serializer.WriteObject(writer, testListeners);
                }

                writer.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    // Close file.
                    writer.Close();
                }
            }
        }

        public static TestListenerCollection DeserializeFromFile(string filePath)
        {
            FileStream reader = null;
            TestListenerCollection testListeners = null;

            try
            {
                // Create DataContractSerializer.
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(typeof(TestListenerCollection));

                // Create a file stream to read into.
                reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Read into object.
                testListeners = serializer.ReadObject(reader) as TestListenerCollection;

                // Fixup assembly path to TestListeners definition.
                foreach (TestListenerDescriptor descriptor in testListeners)
                {
                    TestProperties.ExpandString(descriptor.Assembly);

                    descriptor.Parameters = descriptor.Parameters ?? new Dictionary<string, string>();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    // Close file.
                    reader.Close();
                }
            }

            return testListeners;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TestListeners:  ");

            if (this.Count() > 0)
            {
                foreach (TestListenerDescriptor descriptor in this)
                {
                    sb.AppendLine(descriptor.ToString());
                };
            }
            else
            {
                sb.AppendLine("None defined.");
            }

            return sb.ToString();
        }

        #endregion
    }
}
