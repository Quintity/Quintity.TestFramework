using System;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Runtime
{
    [DataContract]
    public class TestListeners
    {
        #region Class data members

        [DataMember]
        static private TestListenerCollection _testListenersCollection = null;

        public static TestListenerCollection TestListenerCollection
        { get { return _testListenersCollection; } }

        internal static string TestListenersFile
        { get; set; }

        #endregion

        public static void Initialize()
        {
            _testListenersCollection = new TestListenerCollection();
        }

        public static void ReadFromFile(string testListenersFile)
        {

            Uri testListenersUri = new Uri(testListenersFile);
            _testListenersCollection = TestListenerCollection.DeserializeFromFile(testListenersUri.LocalPath);

            // Expand each assembly path.
            foreach(TestListenerDescriptor listener in _testListenersCollection)
            {
                if (listener.Assembly != null)
                {
                    listener.Assembly = TestProperties.ExpandString(listener.Assembly);
                }
            }

            TestListenersFile = testListenersFile;
        }

        public static void Save(string testListenersFile)
        {
            // Expand each assembly path.
            foreach (TestListenerDescriptor listener in _testListenersCollection)
            {
                string testlisteners = TestProperties.TestConfigs;
                listener.Assembly = TestProperties.FixupString(listener.Assembly, "TestListeners");
            }

            TestListenerCollection.SerializeToFile(_testListenersCollection, testListenersFile);
        }
    }
}
