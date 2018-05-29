using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestProfile
    {
        #region Data members

        [DataMember(Order = 0)]
        public int VirtualUsers
        { get; set; }

        [DataMember(Order = 1)]
        public TimeSpan TimeSpan
        { get; set; }

        [DataMember(Order = 2)]
        public string NameFormat
        { get; set; }

        #endregion

        #region Constructors

        public TestProfile()
            : this(1, new TimeSpan(0, 0, 0, 0, 1), "VirtualUser")
        { }

        public TestProfile(int virtualUsers, TimeSpan timeSpan)
            : this(virtualUsers, timeSpan, "VirtualUser")
        { }

        public TestProfile(int virtualUsers, TimeSpan timeSpan, string baseName)
        {
            VirtualUsers = virtualUsers;
            TimeSpan = timeSpan;
            NameFormat = baseName;
        }

        #endregion

        #region Public methods

        public static void WritetoFile(TestProfile TestProfile, string filePath)
        {
            serializeToFile(TestProfile, filePath);
        }

        public static TestProfile ReadFromFile(string filePath)
        {
            return deserializeFromFile(filePath);
        }

        #endregion

        #region Private methods

        private static void serializeToFile(TestProfile TestProfile, string filePath)
        {
            FileStream writer = null;

            try
            {
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(typeof(TestProfile));

                // Create a FileStream to write with.
                writer = new FileStream(filePath, FileMode.Create);

                // Write object out.
                serializer.WriteObject(writer, TestProfile);

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

        private static TestProfile deserializeFromFile(string filePath)
        {
            FileStream reader = null;
            TestProfile TestProfile = null;

            try
            {
                // Create DataContractSerializer.
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(typeof(TestProfile));

                // Create a file stream to read into.
                reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Read into object.
                TestProfile = serializer.ReadObject(reader) as TestProfile;
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

            return TestProfile;
        }

        public override string ToString()
        {
            string format = "Virtual users:  {0}, Timespan: {1}, Base name: {2}";
            return string.Format(format, VirtualUsers, TimeSpan.ToString("G"), NameFormat);
        }

        #endregion
    }
}
