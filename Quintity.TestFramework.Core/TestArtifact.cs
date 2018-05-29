using System;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    abstract public class TestArtifact : TestObject
    {
        #region Class events

        #endregion

        #region Class data members

        [DataMember(Order = 0, IsRequired = true)]
        protected Guid _systemId;

        [CategoryAttribute("Metadata"),
        DisplayName("Identifier"),
        DescriptionAttribute("Test artifact's unique identifier."),
        PropertyOrder(1)]
        virtual public Guid SystemID
        {
            get { return _systemId; }
        }

        #endregion

        #region Class constructors

        public TestArtifact(TestArtifact originalTestArtifact)
            : this()
        { }

        public TestArtifact()
            : base()
        {
            _systemId = Guid.NewGuid();
        }

        #endregion

        #region Class public methods

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Class internal methods

        internal void SetSystemID(Guid systemId) => _systemId = systemId;
       
        #endregion

        #region Class private methods

        #endregion

        #region Class static methods

        internal static void SerializeToFile(TestArtifact testArtifact, string filePath)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(testArtifact.GetType());

                var settings = new XmlWriterSettings() { Indent = true };

                using (var writer = XmlWriter.Create(filePath, settings))
                {
                    serializer.WriteObject(writer, testArtifact);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        internal static TestArtifact DeserializeFromFile(Type type, string filePath)
        {
            FileStream reader = null;
            TestArtifact testArtifact = null;

            try
            {
                // Create DataContractSerializer.
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(type);

                // Create a file stream to read into.
                reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Read into object.
                testArtifact = serializer.ReadObject(reader) as TestArtifact;
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

            return testArtifact;
        }

        #endregion
    }
}
